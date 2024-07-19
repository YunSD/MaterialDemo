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
            //if(entity.MaxQuantity.HasValue) this.MaxQuantity = entity.MaxQuantity.Value;
            //if(entity.MinQuantity.HasValue) this.MinQuantity = entity.MinQuantity.Value;
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
            //this.entity.MaxQuantity = this.MaxQuantity;
            //this.entity.MinQuantity = this.MinQuantity;
            this.entity.Remark = this.Remark;

            SubmitEvent(entity);
        }

    }
}
