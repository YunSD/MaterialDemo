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

        private long? key;

        [Required(ErrorMessage ="该字段不能为空")]
        [ObservableProperty]
        private string? name;

        partial void OnNameChanged(string? value)=>ValidateProperty(value,nameof(Name));


        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? code;
        partial void OnCodeChanged(string? value) =>  ValidateProperty(value, nameof(Code));

        [ObservableProperty]
        public string? model;

        [ObservableProperty]
        private string? unit;


        [ObservableProperty]
        private string? image;

        [Required(ErrorMessage = "该字段不能为空")]
        [Range(minimum:1,maximum:5000, ErrorMessage ="该字段长度至少为 1")]
        [ObservableProperty]
        private int? maxQuantity;
        partial void OnMaxQuantityChanged(int? value) => ValidateProperty(value, nameof(MaxQuantity));


        [Required(ErrorMessage = "该字段不能为空")]
        [Range(minimum: 1, maximum: 5000, ErrorMessage = "该字段长度至少为 1")]
        [ObservableProperty]
        private int? minQuantity;
        partial void OnMinQuantityChanged(int? value) => ValidateProperty(value, nameof(MinQuantity));


        [ObservableProperty]
        private string? remark;


        public delegate bool SaveEventHandler(object sender, DialogOpenedEventArgs eventArgs);

        private FormSubmitEventHandler<StockMaterial> SubmitEvent;

        public StockMaterialEditorViewModel(StockMaterial entity, FormSubmitEventHandler<StockMaterial> submitEvent) {
            
            this.SubmitEvent = submitEvent;
            
            if (entity.MaterialId.HasValue) {
                this.key = entity.MaterialId;
                editModel = false;
            }
            this.Name = entity.Name;
            this.Code = entity.Code;
            this.model = entity.Model;
            this.unit = entity.Unit;
            this.Image = entity.Image;
            this.MaxQuantity = entity.MaxQuantity;
            this.MinQuantity = entity.MinQuantity;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void submit() {
            ValidateAllProperties();
            if (HasErrors) return;

            // image copy
            Image = BaseFileUtil.UpdateFile(Image);

            StockMaterial entity = new()
            {
                MaterialId = this.key,
                Name = this.Name,
                Code = this.Code,
                Model = this.Model,
                Unit = this.Unit,
                Image = this.Image,
                MaxQuantity = this.MaxQuantity,
                MinQuantity = this.MinQuantity,
                Remark = this.Remark
            };

            SubmitEvent(entity);
        }

    }
}
