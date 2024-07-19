using MaterialDemo.Domain;
using MaterialDemo.Domain.Enums;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using System.ComponentModel.DataAnnotations;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class ElectronicTagEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private ElectronicTag entity;


        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? code;
        partial void OnCodeChanged(string? value) => ValidateProperty(value, nameof(Code));

        [RegularExpression(@"^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3})$", ErrorMessage = "请输入正确的IP地址")]
        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public string? ip;

        partial void OnIpChanged(string? value) => ValidateProperty(value, nameof(Ip));

        [Range(minimum: 0, maximum: int.MaxValue, ErrorMessage = "请输入数字")]
        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        public int? address;

        partial void OnAddressChanged(int? value) => ValidateProperty(value, nameof(Address));

        [ObservableProperty]
        public BaseStatusEnum connectStatus = BaseStatusEnum.NORMAL;

        [ObservableProperty]
        public BaseStatusEnum workStatus = BaseStatusEnum.NORMAL;


        [ObservableProperty]
        private string? remark;


        private FormSubmitEventHandler<ElectronicTag> SubmitEvent;

        public ElectronicTagEditorViewModel(ElectronicTag entity, FormSubmitEventHandler<ElectronicTag> submitEvent)
        {

            this.SubmitEvent = submitEvent;
            this.entity = entity;

            if (entity.TagId.HasValue)
            {
                editModel = false;
            }
            this.Code = entity.Code;
            this.Ip = entity.Ip;
            this.Address = entity.Address;
            this.ConnectStatus = entity.ConnectStatus;
            this.WorkStatus = entity.WorkStatus;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void submit()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();
            if (HasErrors) return;

            this.entity.Code = this.Code;
            this.entity.Ip = this.Ip;
            this.entity.Address = this.Address;
            this.entity.ConnectStatus = this.ConnectStatus;
            this.entity.WorkStatus = this.WorkStatus;
            this.entity.Remark = this.Remark;

            SubmitEvent(entity);
        }

    }
}
