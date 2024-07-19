using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Security;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business.VObject;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockIndexItemViewModel : ObservableObject
    {

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


        private readonly IUnitOfWork _unitOfWork;

        public StockIndexItemViewModel(IUnitOfWork unitOfWork, StockIndexItem item)
        {
            _unitOfWork = unitOfWork;

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
            if (LabelSlaveId != null) DataAcquisitionService.Singleton.LabelRequestOpenLight((int)LabelSlaveId, (int)TakeSize);
        }


        [RelayCommand]
        private void CloseView()
        {
            if (LabelSlaveId != null) DataAcquisitionService.Singleton.LabelRequestCloseLight((int)LabelSlaveId);
            DataAcquisitionService.Singleton.VolatileShelfId = -1L;
            DataAcquisitionService.Singleton.ItemChangeNotice -= ItemChangeNotice;
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            DialogHost.Close(BaseConstant.BaseDialog);

            // 记录存取数量
            if (Quantity != CurrentQuantity) Task.Run(saveOperationDetails);
        }

        public void ItemChangeNotice(object? sender, ItemChangeNotice notice)
        {
            if (this.Key == notice.key)
            {
                this.CurrentQuantity = notice.after;
                this.Different = this.CurrentQuantity - Quantity;
            }
        }

        public void saveOperationDetails()
        {

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
}
