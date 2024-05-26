using HotChocolate.Types.Relay;
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

        private long? key;

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


        public delegate bool SaveEventHandler(object sender, DialogOpenedEventArgs eventArgs);

        private FormSubmitEventHandler<StockShelf> SubmitEvent;

        public StockShelfEditorViewModel(StockShelfViewInfo entity, FormSubmitEventHandler<StockShelf> submitEvent) {
            
            this.SubmitEvent = submitEvent;
            
            if (entity.MaterialId.HasValue) {
                this.key = entity.ShelfId;
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

            this.Remark = entity.Remark;

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

            ValidateAllProperties();
            if (HasErrors) return;



            StockShelf entity = new()
            {
                
            };

           // SubmitEvent(entity);
        }

    }
}
