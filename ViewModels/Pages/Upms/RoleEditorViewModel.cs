using MaterialDemo.Domain;
using System.ComponentModel.DataAnnotations;
using MaterialDemo.Utils;
using MaterialDemo.Domain.Models.Entity.Upms;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class RoleEditorViewModel : ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private long? roleId;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? name;

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? code;

        [ObservableProperty]
        private string? remark;


        public delegate bool SaveEventHandler(object sender, DialogOpenedEventArgs eventArgs);

        private FormSubmitEventHandler<SysRole> SubmitEvent;

        public RoleEditorViewModel(SysRole entity, FormSubmitEventHandler<SysRole> submitEvent) {
            this.SubmitEvent = submitEvent;

            if (entity.RoleId.HasValue) {
                this.roleId = entity.RoleId;
                editModel = false;
            }
            this.Name = entity.Name;
            this.Code = entity.Code;
            this.Remark = entity.Remark;
        }

        [RelayCommand]
        private void Submit() {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            ValidateAllProperties();

            if (HasErrors) return;

            SysRole entity = new()
            {
                RoleId = roleId,
                Name = Name,
                Code = Code,
                Remark = Remark,
            };
            SubmitEvent(entity);
        }
    }
}
