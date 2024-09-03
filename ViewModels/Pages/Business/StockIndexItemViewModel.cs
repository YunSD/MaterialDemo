using Linq.PredicateBuilder;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Security;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business.VObject;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockIndexItemViewModel : ObservableObject
    {

        private volatile bool Active = true;

        private readonly long? Key;

        private readonly StockIndexItem Item;
        
        // 电子标签地址
        private readonly int? LabelSlaveId;
        
        [ObservableProperty]
        private string? _MaterialName;
        [ObservableProperty]
        private string? _MaterialCode;
        [ObservableProperty]
        private string? _MaterialModel;
        [ObservableProperty]
        private string? _MaterialImage;
        [ObservableProperty]
        private int? _MaterialWeight;
        [ObservableProperty]
        private int? _MaterialMagnification; 

        [ObservableProperty]
        private int? _TakeSize;
        [ObservableProperty]
        private int? _Quantity;
        [ObservableProperty]
        private int? _CurrentQuantity;
        [ObservableProperty]
        private int? _Different;

        [ObservableProperty]
        private string _DebugInfo = "";

        // 界面稳定指示
        [ObservableProperty]
        private volatile BaseStatusEnum status = BaseStatusEnum.EXCEPTION;

        // 临近点指示
        [ObservableProperty]
        private volatile BaseStatusEnum warning = BaseStatusEnum.NORMAL;


        // 间隔
        private static readonly int Interval = 3;
        private volatile int Frequency = Interval;

        // 初次界面重量
        private int Weight;

        public StockIndexItemViewModel(StockIndexItem item, int? Weight)
        {
            Key = item.ShelfId;
            Item = item;

            MaterialName = item.StockMaterial?.Name;
            MaterialCode = item.StockMaterial?.Code;
            MaterialModel = item.StockMaterial?.Model;
            MaterialImage = item.StockMaterial?.Image;
            MaterialWeight = item.StockMaterial?.Weight;
            _MaterialMagnification = item.StockMaterial?.Magnification;
            TakeSize = item.TakeSize;
            Quantity = item.Quantity;
            CurrentQuantity = item.Quantity;

            LabelSlaveId = item.ElectronicTag?.Address;
            
            if (Weight == null) { this.Weight = 0; } else { this.Weight = Weight.Value; }  

            // 注册通知
            DataAcquisitionService.Singleton.WeightChangeNotice += ItemWeightChangeNotice;
            if (item.ShelfId != null) DataAcquisitionService.Singleton.VolatileShelfId = (long)item.ShelfId;
            // 点亮标签
            //if (LabelSlaveId != null) DataAcquisitionService.Singleton.LabelRequestOpenLight((int)LabelSlaveId, (int)TakeSize);
        }


        [RelayCommand]
        private void CloseView()
        {
            lock(this){
                Active = false;
                DataAcquisitionService.Singleton.VolatileShelfId = -1L;
                DataAcquisitionService.Singleton.ItemChangeNotice -= ItemWeightChangeNotice;
                if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
                DialogHost.Close(BaseConstant.BaseDialog);

                Task.Run(() => {
                    // 记录存取数量
                    SaveOperationDetails();
                    // 再次更新标签数量
                    if (Item.ShelfId != null && LabelSlaveId != null)
                        _ = DataAcquisitionService.Singleton.LabelRequestEditAcquiredCount((long)Item.ShelfId, (int)LabelSlaveId);
                });
            }
        }

        /// <summary>
        /// 重量发生变动时进行通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="notice"></param>
        public void ItemWeightChangeNotice(object? sender, ItemChangeNotice notice)
        {
            lock (this) {
                int? OLD_DIFFERENT = 0;
                if (this.Different != null) OLD_DIFFERENT = this.Different;
                if (this.Weight == 0) this.Weight = notice.Weight;
                if (this.Key == notice.Key && Active)
                {
                    int weight_different = Weight - notice.Weight;
                    
                    // 判断正负号, 默认为正数, 这里需要区分拿取与放入
                    // 放入较少零件时，一次放入的重量可能会被分解为多次微小重量波动，导致无法区分起始点。
                    bool isTake = true;
                    if (weight_different < 0) isTake = false;

                    if (isTake) {
                        // 拿取过程采用计重运算，因为人手介入时波动较大
                        int dif = DataAcquisitionService.CalculatePartNumber(Math.Abs(weight_different), MaterialWeight.Value, MaterialMagnification.Value);
                        this.Different = 0 - dif;
                    }
                    else
                    {
                        this.Different = notice.After - Quantity;
                    }

                    if (OLD_DIFFERENT == Different) {
                        // frequency
                        if(Frequency != 0) Frequency--;
                        if (Frequency <= 0)
                        {
                            Status = BaseStatusEnum.NORMAL;
                            Frequency = Interval;
                            DebugInfo = string.Format("总重: {0}, 当前重: {1}, 重量差: {2}000, 单体重: {3} , {4}", Weight, notice.Weight, weight_different, MaterialWeight, Math.Round((double)(weight_different * 1000.00 / MaterialWeight),3));

                            // 判断当前重量值是否处于临界值 ±5%
                            if (isTake) {
                                int remainder = weight_different * 1000 % MaterialWeight.Value;
                                if (Math.Abs(MaterialWeight.Value * MaterialMagnification.Value * 0.01 - remainder) <= MaterialWeight.Value * 0.03)
                                {
                                    Warning = BaseStatusEnum.EXCEPTION;
                                }
                                else
                                {
                                    Warning = BaseStatusEnum.NORMAL;
                                }
                            }
                        }
                    }
                    else
                    {
                        CurrentQuantity = Quantity + Different;
                        _ = DataAcquisitionService.Singleton.LabelRequestEditDifferentCount((int)LabelSlaveId, Math.Abs((int)this.Different));
                        Frequency = Interval;
                        Status = BaseStatusEnum.EXCEPTION;
                    }
                }
            }
        }

        public void SaveOperationDetails()
        {
            // 未变动情况下不再记录
            if (CurrentQuantity - Quantity == 0) return;
            using (IServiceScope scope = App.CreateServiceScope()) {
                IUnitOfWork? _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                if (_unitOfWork == null) return;
                var statement_repository = _unitOfWork.GetRepository<StockMaterialStatement>(true);
                var exception_repository = _unitOfWork.GetRepository<StockException>(true);

                StockMaterialStatement statement = new StockMaterialStatement
                {
                    StatementId = SnowflakeIdWorker.Singleton.nextId(),
                    MaterialName = this.Item.StockMaterial?.Name,
                    MaterialCode = this.Item.StockMaterial?.Code,
                    MaterialModel = this.Item.StockMaterial?.Model,
                    MaterialUnit = this.Item.StockMaterial?.Unit,
                    ShelfInfo = this.Item.WarehouseName + "-" + this.Item.ShelvesCode + "-" + this.Item.Code,
                    Type = MaterialStatementTypeEnum.TAKE,
                    Way = MaterialStatementWayEnum.NORMAL, 
                    BeforeStock = Convert.ToString(Quantity),
                    AfterStock = Convert.ToString(CurrentQuantity),
                    Amount = Convert.ToString(Math.Abs((int)(Quantity - CurrentQuantity))),
                    OperatorName = SecurityContext.GetUserName(),
                };

                if (CurrentQuantity > Quantity) statement.Type = MaterialStatementTypeEnum.SAVE;

                // 记录存取信息
                statement_repository.Insert(statement);

                // 在当前数量少于原数量，且实际取用量与计划取用量不符时归入异常并记录
                if (this.TakeSize != Quantity - CurrentQuantity && Quantity > CurrentQuantity)
                {
                    StockException stockException = new StockException
                    {
                        ExceptionId = SnowflakeIdWorker.Singleton.nextId(),
                        MaterialName = this.Item.StockMaterial?.Name,
                        MaterialCode = this.Item.StockMaterial?.Code,
                        MaterialModel = this.Item.StockMaterial?.Model,
                        MaterialUnit = this.Item.StockMaterial?.Unit,
                        ShelfInfo = this.Item.WarehouseName + "-" + this.Item.ShelvesCode + "-" + this.Item.Code,
                        Type = StockExceptionTypeEnum.TAKE,
                        BeforeStock = Convert.ToString(Quantity),
                        AfterStock = Convert.ToString(CurrentQuantity),
                        Amount = Convert.ToString(Math.Abs((int)(Quantity - CurrentQuantity))),
                        OperatorName = SecurityContext.GetUserName(),
                    };
                    exception_repository.Insert(stockException);
                }

                // 在当前数量低于上下限的时候触发
                if (Quantity > this.Item.QuantityUpperLimit || Quantity < Item.QuantityLowerLimit)
                {

                    StockException stockException = new StockException
                    {
                        ExceptionId = SnowflakeIdWorker.Singleton.nextId(),
                        MaterialName = this.Item.StockMaterial?.Name,
                        MaterialCode = this.Item.StockMaterial?.Code,
                        MaterialModel = this.Item.StockMaterial?.Model,
                        MaterialUnit = this.Item.StockMaterial?.Unit,
                        ShelfInfo = this.Item.WarehouseName + "-" + this.Item.ShelvesCode + "-" + this.Item.Code,
                        Type = StockExceptionTypeEnum.LOWER_LIMIT,
                        BeforeStock = Convert.ToString(Quantity),
                        AfterStock = Convert.ToString(CurrentQuantity),
                        Amount = Convert.ToString(Math.Abs((int)(Quantity - CurrentQuantity))),
                        OperatorName = SecurityContext.GetUserName(),
                    };
                    if (Quantity > this.Item.QuantityUpperLimit) stockException.Type = StockExceptionTypeEnum.UPPER_LIMIT;
                    exception_repository.Insert(stockException);
                }

                _unitOfWork.SaveChanges();
                _unitOfWork.TrackClear();
            }
        }

        /// <summary>
        /// 废弃方法
        /// </summary>
        /// <param name="stockMaterial"></param>
        public void SaveMaterialDynamicCalibration(StockMaterial stockMaterial)
        {
            using (IServiceScope scope = App.CreateServiceScope())
            {
                IUnitOfWork? _unitOfWork = scope.ServiceProvider.GetService<IUnitOfWork>();
                if (_unitOfWork == null) return;
                var material_repository = _unitOfWork.GetRepository<StockMaterial>(true);
                material_repository.Update(stockMaterial);
                material_repository.excludeEntityField(stockMaterial, [nameof(StockMaterial.Weight)]);
                _unitOfWork.SaveChanges();
                _unitOfWork.TrackClear();
            }
        }
    }
}
