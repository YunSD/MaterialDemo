using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using System.ComponentModel.DataAnnotations;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockMaterialEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private StockMaterial entity;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? name;

        partial void OnNameChanged(string? value) => ValidateProperty(value, nameof(Name));


        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? code;
        partial void OnCodeChanged(string? value) => ValidateProperty(value, nameof(Code));

        [ObservableProperty]
        public string? model;

        [ObservableProperty]
        private string? unit;


        [ObservableProperty]
        private string? image;

        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "请输入数字")]
        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private int? weight;
        partial void OnWeightChanged(int? value) => ValidateProperty(value, nameof(Weight));

        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "请输入数字: 0-100")]
        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private int? magnification;
        partial void OnMagnificationChanged(int? value) => ValidateProperty(value, nameof(Magnification));

        //[GreaterThan(nameof(MinQuantity), ErrorMessage ="数量上限不能低于数量下限")]
        //[Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "请输入数字")]
        //[ObservableProperty]
        //private int? maxQuantity = 0;
        //partial void OnMaxQuantityChanged(int? value) => ValidateProperty(value, nameof(MaxQuantity));


        //[Range(minimum: 0, int.MaxValue, ErrorMessage = "请输入数字")]
        //[ObservableProperty]
        //private int? minQuantity = 0;
        //partial void OnMinQuantityChanged(int? value) => ValidateProperty(value, nameof(MinQuantity));


        [ObservableProperty]
        private string? remark;


        private FormSubmitEventHandler<StockMaterial> SubmitEvent;

        public StockMaterialEditorViewModel(StockMaterial entity, FormSubmitEventHandler<StockMaterial> submitEvent)
        {

            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.MaterialId.HasValue)
            {
                editModel = false;
            }

            this.Name = entity.Name;
            this.Code = entity.Code;
            this.model = entity.Model;
            this.unit = entity.Unit;
            this.Image = entity.Image;
            this.Weight = entity.Weight;
            this.Magnification = entity.Magnification;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void submit()
        {

            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;

            ValidateAllProperties();
            if (HasErrors) return;

            this.entity.Name = this.Name;
            this.entity.Code = this.Code;
            this.entity.Model = this.Model;
            this.entity.Unit = this.Unit;
            this.entity.Image = this.Image;
            this.entity.Weight = this.Weight;
            this.entity.Magnification = this.Magnification;
            this.entity.Remark = this.Remark;

            SubmitEvent(entity);
        }

    }
}
