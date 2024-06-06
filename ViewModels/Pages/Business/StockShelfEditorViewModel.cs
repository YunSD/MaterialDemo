using MaterialDemo.Domain;
using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.Utils.Validation;
using MaterialDemo.ViewModels.Pages.Business.VObject;
using System.ComponentModel.DataAnnotations;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockShelfEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private StockShelfViewInfo entity;

        [Required(ErrorMessage ="该字段不能为空")]
        [ObservableProperty]
        private string? code;
        partial void OnCodeChanged(string? value) => ValidateProperty(value, nameof(Code));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? barCode;
        partial void OnBarCodeChanged(string? value) => ValidateProperty(value, nameof(BarCode));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? warehouseName;
        partial void OnWarehouseNameChanged(string? value) => ValidateProperty(value, nameof(WarehouseName));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? shelvesCode;
        partial void OnShelvesCodeChanged(string? value) => ValidateProperty(value, nameof(ShelvesCode));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? shelvesType;
        partial void OnShelvesTypeChanged(string? value) => ValidateProperty(value, nameof(ShelvesType));

        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "请输入数字")]
        [ObservableProperty]
        public int? quantity;
        partial void OnQuantityChanged(int? value) => ValidateProperty(value, nameof(Quantity));

        [Range(minimum: 1, maximum: int.MaxValue, ErrorMessage = "请输入数字")]
        [ObservableProperty]
        public int? takeSize;
        partial void OnTakeSizeChanged(int? value) => ValidateProperty(value, nameof(TakeSize));

        [GreaterThan(nameof(QuantityLowerLimit), ErrorMessage = "数量上限不能低于数量下限")]
        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "请输入数字")]
        [ObservableProperty]
        private int? quantityUpperLimit = 0;
        partial void OnQuantityUpperLimitChanged(int? value) => ValidateProperty(value, nameof(QuantityUpperLimit));


        [Range(minimum: 0, int.MaxValue, ErrorMessage = "请输入数字")]
        [ObservableProperty]
        private int? quantityLowerLimit = 0;
        partial void OnQuantityLowerLimitChanged(int? value) => ValidateProperty(value, nameof(QuantityLowerLimit));


        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "请输入数字")]
        [ObservableProperty]
        public int? scalesAddress;
        partial void OnScalesAddressChanged(int? value) => ValidateProperty(value, nameof(ScalesAddress));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public BaseStatusEnum scalesSatus = BaseStatusEnum.NORMAL;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? scalesModel;
        partial void OnScalesModelChanged(string? value) => ValidateProperty(value, nameof(ScalesModel));

        [ObservableProperty]
        private string? remark;

        [ObservableProperty]
        private long? stockMaterialId;

        [ObservableProperty]
        private string? stockMaterialInfo;

        [ObservableProperty]
        private long? electronicTagId;

        [ObservableProperty]
        private string? electronicTagInfo;


        private FormSubmitEventHandler<StockShelf> SubmitEvent;

        public StockShelfEditorViewModel(StockShelfViewInfo entity, FormSubmitEventHandler<StockShelf> submitEvent) {
            
            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.ShelfId.HasValue) {
                editModel = false;
            }
            this.Code = entity.Code;
            this.BarCode = entity.BarCode;

            this.WarehouseName = entity.WarehouseName;
            this.ShelvesCode = entity.ShelvesCode;
            this.ShelvesType = entity.ShelvesType;

            this.ScalesAddress = entity.ScalesAddress;
            this.ScalesModel = entity.ScalesModel;
            this.ScalesSatus = entity.ScalesStatus;

            this.Quantity = entity.Quantity;   
            this.Remark = entity.Remark;

            if(entity.QuantityUpperLimit.HasValue) this.QuantityUpperLimit = entity.QuantityUpperLimit.Value;
            if(entity.QuantityLowerLimit.HasValue) this.QuantityLowerLimit = entity.QuantityLowerLimit.Value;
            if(entity.TakeSize.HasValue) this.TakeSize = entity.TakeSize.Value;

            if (entity.StockMaterial != null) {
                this.StockMaterialId = entity.StockMaterial.MaterialId;
                this.StockMaterialInfo = entity.StockMaterial.Name;
            }
            if (entity.ElectronicTag != null)
            {
                this.ElectronicTagId = entity.ElectronicTag.TagId;
                this.ElectronicTagInfo = entity.ElectronicTag.Code;
            }
        }

        [RelayCommand]
        private void submit() {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();
            if (HasErrors) return;

            this.entity.MaterialId = this.StockMaterialId;
            this.entity.TagId = this.ElectronicTagId;

            this.entity.Code = this.Code;
            this.entity.BarCode = this.BarCode;
            this.entity.WarehouseName = this.WarehouseName;
            this.entity.ShelvesCode = this.ShelvesCode;
            this.entity.ShelvesType = this.ShelvesType;
            this.entity.Quantity = this.Quantity;
            this.entity.QuantityUpperLimit = this.QuantityUpperLimit;
            this.entity.QuantityLowerLimit = this.QuantityLowerLimit;
            this.entity.TakeSize = this.TakeSize;

            this.entity.ScalesAddress = this.ScalesAddress;
            this.entity.ScalesModel = this.ScalesModel;
            this.entity.ScalesStatus = this.ScalesSatus;

            this.entity.Remark = this.Remark;

            SubmitEvent(entity);
        }

    }
}
