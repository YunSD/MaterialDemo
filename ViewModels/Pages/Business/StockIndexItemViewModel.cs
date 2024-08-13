using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Security;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business.VObject;
using System.Windows.Media.Media3D;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockIndexItemViewModel : ObservableObject
    {

        private volatile bool Active = true;

        private readonly long? Key;
        private readonly StockIndexItem Item;

        [ObservableProperty]
        private string? _MaterialName;
        [ObservableProperty]
        private string? _MaterialCode;
        [ObservableProperty]
        private string? _MaterialModel;
        [ObservableProperty]
        private string? _MaterialImage;

        [ObservableProperty]
        private int? _TakeSize;
        [ObservableProperty]
        private int? _Quantity;
        [ObservableProperty]
        private int? _CurrentQuantity;
        [ObservableProperty]
        private int? _Different;

        private readonly int? LabelSlaveId;

        [ObservableProperty]
        private volatile BaseStatusEnum status = BaseStatusEnum.EXCEPTION;

        private static readonly int Interval = 5;

        private volatile int Frequency = Interval;

        public StockIndexItemViewModel(StockIndexItem item)
        {
            Key = item.ShelfId;
            Item = item;

            MaterialName = item.StockMaterial?.Name;
            MaterialCode = item.StockMaterial?.Code;
            MaterialModel = item.StockMaterial?.Model;
            MaterialImage = item.StockMaterial?.Image;
            TakeSize = item.TakeSize;
            Quantity = item.Quantity;
            CurrentQuantity = item.Quantity;

            LabelSlaveId = item.ElectronicTag?.Address;

            // 注册通知
            if (item.ShelfId != null) DataAcquisitionService.Singleton.VolatileShelfId = (long)item.ShelfId;
            DataAcquisitionService.Singleton.ItemChangeNotice += ItemChangeNotice;
            // 点亮标签
            //if (LabelSlaveId != null) DataAcquisitionService.Singleton.LabelRequestOpenLight((int)LabelSlaveId, (int)TakeSize);
        }


        [RelayCommand]
        private void CloseView()
        {
            lock(this){
                //if (LabelSlaveId != null) DataAcquisitionService.Singleton.LabelRequestCloseLight((int)LabelSlaveId);
                Active = false;
                DataAcquisitionService.Singleton.VolatileShelfId = -1L;
                DataAcquisitionService.Singleton.ItemChangeNotice -= ItemChangeNotice;
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

        public void ItemChangeNotice(object? sender, ItemChangeNotice notice)
        {
            lock (this) {
                int? OLD_DIFFERENT = 0;
                if (this.Different != null) OLD_DIFFERENT = this.Different;
                if (this.Key == notice.key && Active)
                {
                    this.CurrentQuantity = notice.after;
                    this.Different = this.CurrentQuantity - Quantity;
                   
                    if (OLD_DIFFERENT == Different) {
                        // frequency
                        if(Frequency != 0) Frequency--;
                        if (Frequency <= 0)
                        {
                            Status = BaseStatusEnum.NORMAL;
                            Frequency = Interval;
                            if (MaterialDynamicCalibrationEnum.OPEN == Item.StockMaterial?.DynamicCalibration)
                            {
                                // 触发更新测量
                                int cur_weight = (int)Math.Abs(notice.weight * 1000 / notice.after);

                                if (cur_weight > Item.StockMaterial?.Weight)
                                {
                                    if(cur_weight - Item.StockMaterial?.Weight < 1) return;
                                    //int dif = (int)(cur_weight - Item.StockMaterial?.Weight);
                                    //if (dif > 5) { dif = 5; }
                                    Item.StockMaterial.Weight = (int)Item.StockMaterial?.Weight + 1;
                                }
                                else {
                                    if (Item.StockMaterial?.Weight - cur_weight < 1) return;
                                    //int dif = (int)(Item.StockMaterial?.Weight - cur_weight);
                                    //if (dif > 5) { dif = 5; }
                                    Item.StockMaterial.Weight = (int)Item.StockMaterial?.Weight - 1;
                                }
                                
                                // 通知更改
                                DataAcquisitionService.Singleton.UpdateMaterialWeight(Item.StockMaterial);
                                // 存储当前值
                                SaveMaterialDynamicCalibration(Item.StockMaterial);
                            }
                        }
                    }
                    else
                    {
                        _ = DataAcquisitionService.Singleton.LabelRequestEditDifferentCount((int)LabelSlaveId, Math.Abs((int)this.Different));
                        Frequency = Interval;
                        Status = BaseStatusEnum.EXCEPTION;
                    }
                }
            }
        }

        public void SaveOperationDetails()
        {
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
