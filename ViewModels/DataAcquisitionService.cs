using HI.Label;
using HI.Scale;
using Linq.PredicateBuilder;
using log4net;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Security;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business.VObject;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;

namespace MaterialDemo.ViewModels
{
    public class DataAcquisitionService
    {
        private static readonly ILog logger = LogManager.GetLogger(nameof(DataAcquisitionService));

        private static readonly DataAcquisitionService DATA_ACQUISITION = new DataAcquisitionService();

        public static DataAcquisitionService Singleton { get { return DATA_ACQUISITION; } }

        private readonly Thread WorkThread;

        private volatile bool WorkDone = false;

        private static readonly ConcurrentDictionary<long, int> NumberOfPartPairs = new();

        private static readonly ConcurrentDictionary<long, ElectronicTag> ElectronicTagKeyPairs = new();

        private static readonly ConcurrentDictionary<long, StockShelf> StockShelfKeyPairs = new();

        private static readonly ConcurrentDictionary<long, StockMaterial> StockMaterialKeyPairs = new();

        private readonly ScaleClient _ScaleClient;

        private readonly ETagClient _ETagClient;

        private DataAcquisitionService()
        {
            try
            {
                this.WorkThread = new Thread(CollectTask);

                string? SCALE_COM_ADDR = System.Configuration.ConfigurationManager.AppSettings["SCALE_COM_ADDR"];
                string? ETAG_CONTROL_ADDR = System.Configuration.ConfigurationManager.AppSettings["ETAG_CONTROL_ADDR"];

                _ScaleClient = new ScaleClient(SCALE_COM_ADDR);
                _ETagClient = new ETagClient(new IPEndPoint(IPAddress.Parse(ETAG_CONTROL_ADDR.Split(":")[0]), int.Parse(ETAG_CONTROL_ADDR.Split(":")[1])));

                Task.Delay(1000).ContinueWith((t) => this.loadBasicData());
            }
            catch (Exception e)
            {
                throw new Exception("数据采集服务初始化失败，请检查配置文件后重试。");
            }

        }

        /// <summary>
        /// 启动采集
        /// </summary>
        public void StartUp()
        {
            if (!this.WorkDone)
            {
                this.WorkDone = true;
                this.WorkThread.Start();
                this._ScaleClient.Connect();
                this._ETagClient.Connect();
            }
        }

        /// <summary>
        /// 结束采集并关闭线程
        /// </summary>
        public void Shutdown()
        {
            this.WorkDone = false;
            this._ScaleClient.Disconnect();
            this._ETagClient.Disconnect();
        }

        /// <summary>
        /// 工作状态查询
        /// </summary>
        /// <returns> bool：状态 </returns>
        public bool RequestEtagConnectStatus() => _ETagClient.RequestConnectStatus();
        public bool RequestScaleConnectStatus() => _ScaleClient.RequestConnectStatus();
        public bool RequestWorkeerStatus() => this.WorkDone;


        /// <summary>
        /// 收集数据并动态触发更新操作
        /// </summary>
        public async void CollectTask()
        {
            while (WorkDone)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                try
                {
                    if (this._ScaleClient.RequestConnectStatus())
                    {
                        IList<ItemChangeNotice> changeNotices = [];

                        foreach (var item in StockShelfKeyPairs.Values)
                        {
                            while (WorkDone) break;
                            long key = (long)item.ShelfId;

                            int newValue = 0;
                            int weight = 0;
                            if (item.ScalesAddress != null)
                            {
                                weight = await _ScaleClient.RequestWeightOfParts((int)item.ScalesAddress);
                                //newValue = await _ScaleClient.RequestNumberOfParts((int)item.ScalesAddress);
                                //newValue = (new Random()).Next(10,12);
                                // 重量换算
                                if (item.MaterialId != null && StockMaterialKeyPairs.TryGetValue((long)item.MaterialId, out StockMaterial? material)){
                                    if (material != null && material.Weight != null && material.Magnification != null) {
                                        newValue = CalculatePartNumber(weight, (int)material.Weight, (int)material.Magnification);
                                    }
                                }
                            }

                            if (NumberOfPartPairs.TryGetValue(key, out int oldValue))
                            {
                                // notice
                                ItemChangeNotice itemChange = new ItemChangeNotice(key, oldValue, newValue, weight);
                                m_ItemChangeNotice?.Invoke(this, itemChange);
                                if (newValue != oldValue) {
                                    changeNotices.Add(itemChange);
                                    // update pair
                                    NumberOfPartPairs.TryUpdate(key, newValue, oldValue);
                                    // update etag
                                    if (ElectronicTagKeyPairs.TryGetValue((long)item.TagId, out ElectronicTag? tag))
                                    {
                                        if (tag != null && tag.Address != null && _ETagClient.RequestConnectStatus() && VolatileShelfId != key)
                                        {
                                            _ETagClient.RequestUpdateCount((int)tag.Address, newValue);
                                        }
                                    }
                                }
                            }
                        }

                        if (changeNotices.Any()) this.SaveChangeInfo(changeNotices);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }
                stopwatch.Stop();
                //logger.DebugFormat("测量传感器采集执行时间：{0}", stopwatch.Elapsed);
                Thread.Sleep(100);
            }
        }


        /// <summary>
        /// 异步加载基础数据
        /// </summary>
        /// <returns></returns>
        public async Task loadBasicData()
        {
            DataAcquisitionService.ElectronicTagKeyPairs.Clear();
            DataAcquisitionService.StockShelfKeyPairs.Clear();
            DataAcquisitionService.StockMaterialKeyPairs.Clear();
            DataAcquisitionService.NumberOfPartPairs.Clear();

            IUnitOfWork? _unitOfWork = App.GetService<IUnitOfWork>();
            if (_unitOfWork == null) return;

            var tags = (await (_unitOfWork.GetRepository<ElectronicTag>(true).GetAllAsync())).ToList();
            var materials = (await _unitOfWork.GetRepository<StockMaterial>(true).GetAllAsync()).ToList();
            var shelves = (await (_unitOfWork.GetRepository<StockShelf>(true).GetAllAsync())).ToList();

            tags.ForEach(t => ElectronicTagKeyPairs.TryAdd((long)t.TagId, t));
            materials.ForEach(t => StockMaterialKeyPairs.TryAdd((long)t.MaterialId, t));
            shelves.ForEach(t =>
            {
                StockShelfKeyPairs.TryAdd((long)t.ShelfId, t);
                NumberOfPartPairs.AddOrUpdate((long)t.ShelfId, (int)t.Quantity, (existingKey, existingValue) => existingValue);
            });

            await Task.CompletedTask;
        }
        /// <summary>
        /// 更新指定的物料信息
        /// </summary>
        /// <param name="stockMaterial"></param>
        public void UpdateMaterialWeight(StockMaterial stockMaterial) {
            if (stockMaterial.MaterialId == null) return;
            if(StockMaterialKeyPairs.TryGetValue((long)stockMaterial.MaterialId, out StockMaterial? material)){
                StockMaterialKeyPairs.TryUpdate((long)stockMaterial.MaterialId, material, stockMaterial);
            }
        }


        private long _ShelfId;

        public long VolatileShelfId
        {
            get => Volatile.Read(ref _ShelfId);
            set => Volatile.Write(ref _ShelfId, value);
        }

        /// <summary>
        /// 该事件为通知保底操作，当物料数量发生变化时，判断当前物料是否正常操作
        /// 如果非正常操作，那么就按照异常进行记录，如果为正常操作，就停止记录
        /// 正常情况下，同一时间只能操作一类物料
        /// </summary>
        /// <returns></returns>
        /// 
        public void SaveChangeInfo(IList<ItemChangeNotice> notice)
        {
            // 执行记录与保存
            Task.Run(() =>
            {
                List<StockMaterialStatement> statementsSet = [];
                List<StockShelf> shelvesSet = [];
                foreach (var item in notice)
                {
                    StockShelf? shelf = StockShelfKeyPairs.GetValueOrDefault(item.key, null);
                    if (shelf == null || shelf.MaterialId == null) continue;
                    StockMaterial? material = StockMaterialKeyPairs.GetValueOrDefault((long)shelf.MaterialId, null);
                    if (material == null) continue;

                    StockMaterialStatement statement = new();
                    statement.StatementId = SnowflakeIdWorker.Singleton.nextId();
                    statement.MaterialName = material.Name;
                    statement.MaterialCode = material.Code;
                    statement.MaterialModel = material.Model;
                    statement.MaterialUnit = material.Unit;
                    statement.ShelfInfo = shelf.WarehouseName + "-" + shelf.ShelvesCode + "-" + shelf.Code;
                    statement.Type = MaterialStatementTypeEnum.TAKE;
                    if (item.after > item.before) statement.Type = MaterialStatementTypeEnum.SAVE;
                    statement.BeforeStock = Convert.ToString(item.before);
                    statement.AfterStock = Convert.ToString(item.after);
                    statement.Amount = Convert.ToString(item.value);
                    statement.OperatorName = SecurityContext.GetUserName();
                    if (item.key != VolatileShelfId) statementsSet.Add(statement);

                    shelf.Quantity = item.after;
                    shelvesSet.Add(shelf);
                }

                try
                {
                    using (IServiceScope scope = App.CreateServiceScope())
                    {
                        IUnitOfWork? _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                        if (_unitOfWork == null) return;

                        var statement_repository = _unitOfWork.GetRepository<StockMaterialStatement>(true);
                        statement_repository.Insert(statementsSet);

                        var shelves_repository = _unitOfWork.GetRepository<StockShelf>(true);
                        shelves_repository.Update(shelvesSet);

                        shelves_repository.excludeEntityField(shelvesSet, [nameof(StockShelf.Quantity)]);

                        _unitOfWork.SaveChanges();

                        _unitOfWork.TrackClear();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                }

            });
        }


        /// <summary>
        /// 外部注册的通知事件管理器
        /// </summary>
        private event EventHandler<ItemChangeNotice>? m_ItemChangeNotice;
        public event EventHandler<ItemChangeNotice> ItemChangeNotice
        {
            add { m_ItemChangeNotice += value; }
            remove { m_ItemChangeNotice -= value; }
        }

        #region hardware operation

        /// <summary>
        /// 点亮电子标签
        /// </summary>
        /// <param name="index">slaveId</param>
        /// <param name="amount">数量</param>
        public void LabelRequestOpenLight(int index, int amount)
        {
            _ETagClient.RequestOpenLight(index, amount);
        }

        /// <summary>
        /// 关闭电子标签
        /// </summary>
        /// <param name="index">slaveId</param>
        public void LabelRequestCloseLight(int index)
        {
            _ETagClient.RequestCloseLight(index);
        }

        /// <summary>
        /// 通过查询值更新电子标签数量
        /// </summary>
        /// <param name="index">slaveId</param>
        public async Task LabelRequestEditAcquiredCount(long shelfId, int tagIndex) {
            if (NumberOfPartPairs.TryGetValue(shelfId, out int count)) {
                logger.Debug("通过查询值更新电子标签数量" + count);
                _ETagClient.RequestUpdateCount(tagIndex, count);
            }
            await Task.CompletedTask;
        }

        /// <summary>
        /// 通过差异值更新电子标签数量
        /// </summary>
        /// <param name="shelfId"></param>
        /// <param name="tagIndex"></param>
        /// <returns></returns>
        public async Task LabelRequestEditDifferentCount(int tagIndex, int count)
        {
            logger.Debug("通过差异值更新电子标签数量" + count);
            _ETagClient.RequestUpdateCount(tagIndex, count, false);
            await Task.CompletedTask;
        }


        /// <summary>
        /// 更新电子标签内容
        /// </summary>
        /// <param name="stockShelves"></param>
        public async Task LabelRequestTextContent(StockShelfViewInfo stockShelves)
        {
            await this.LabelRequestTextContent([stockShelves]);
        }

        /// <summary>
        /// 批量更新电子标签内容
        /// </summary>
        /// <param name="stockShelves"></param>
        public async Task LabelRequestTextContent(IList<StockShelfViewInfo> stockShelves)
        {
            if (!stockShelves.Any()) return;
            await Task.Run(() =>
            {
                foreach (var item in stockShelves)
                {
                    int? slaveId = item.ElectronicTag?.Address;
                    string? name = item.StockMaterial?.Name;
                    string? model = item.StockMaterial?.Model;
                    string? shelf_code = item.StockMaterial.Code;
                    if (slaveId == null) continue;
                    _ETagClient.RequestTextContent((int)slaveId, name, model, shelf_code);
                    _ETagClient.RequestUpdateCount((int)slaveId, item.Quantity != null ? (int)item.Quantity: 0);
                }
            });
        }


        /// <summary>
        /// 读取秤传感器所有重要数据
        /// </summary>
        /// <param name="slaveId"></param>
        /// <returns></returns>
        public async Task<IList<int>> ScaleRequestAllData(int slaveId) {
            IList<int> result = await _ScaleClient.RequestAllData(slaveId);
            return result;
        }
        /// <summary>
        /// 零点标定
        /// </summary>
        /// <param name="slaveId"></param>
        /// <returns></returns>
        public async Task<bool> RequestWriteZeroDemarcate(int slaveId)
        {
            return await _ScaleClient.RequestWriteZeroDemarcate(slaveId);
        }

        /// <summary>
        /// 满载标定
        /// </summary>
        /// <param name="slaveId"></param>
        /// <returns></returns>
        public async Task<bool> RequestWriteFullDemarcate(int slaveId)
        {
            return await _ScaleClient.RequestWriteFullDemarcate(slaveId);
        }

        /// <summary>
        /// 砝码标定
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public async Task<bool> RequestWriteWeightDemarcate(int slaveId, int weight)
        {
            return await _ScaleClient.RequestWriteWeightDemarcate(slaveId, weight);
        }

        /// <summary>
        /// 计数开关
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public async Task<bool> RequestWriteCountSwitch(int slaveId, int flag)
        {
            return await _ScaleClient.RequestWriteCountSwitch(slaveId, flag);
        }

        /// <summary>
        /// 单体倍率
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="magnification"></param>
        /// <returns></returns>
        public async Task<bool> RequestWriteMonomerMagnification(int slaveId, int magnification)
        {
            return await _ScaleClient.RequestWriteMonomerMagnification(slaveId, magnification);
        }

        /// <summary>
        /// 单体重量
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        public async Task<bool> RequestWriteMonomerWeight(int slaveId, int weight)
        {
            return await _ScaleClient.RequestWriteMonomerWeight(slaveId, weight);
        }

        /// <summary>
        /// 当前数量
        /// </summary>
        /// <param name="slaveId"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public async Task<bool> RequestWriteCurrentCount(int slaveId, int count)
        {
            return await _ScaleClient.RequestWriteCurrentCount(slaveId, count);
        }

        #endregion


        private static int CalculatePartNumber(int weight, int m_weight, int naginication) {
            if (weight <= 0 || m_weight < 1) return 0;
            weight = weight * 1000;
            int number = (int) Math.Abs(weight * (1.0 / m_weight));
            
            if (m_weight * naginication / 100 < weight % m_weight) {
                number++;
            }

            return number; 
        }
    }


    public class ItemChangeNotice
    {
        public ItemChangeNotice(long key, int before, int after, int weight)
        {
            
            this.key = key;
            this.before = before;
            this.after = after;
            this.value = Math.Abs(before - after);
            this.weight = weight;
        }

        
        public long key { get; set; }
        public int value { get; set; }
        public int before { get; set; }
        public int after { get; set; }
        public long weight { get; set; }
    }
}
