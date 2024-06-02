using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity;
using System.ComponentModel.DataAnnotations;
using MaterialDemo.Domain.Enums;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class UserEditorViewModel:ObservableValidator
    {
        [ObservableProperty]
        private bool editModel = true;

        private long? userId;

        [Required(ErrorMessage ="该字段不能为空")]
        [ObservableProperty]
        private string? username;

        partial void OnUsernameChanged(string? value)=>ValidateProperty(value,nameof(Username));

        [Required(ErrorMessage = "该字段不能为空")]
        [ObservableProperty]
        private string? name;
        partial void OnNameChanged(string? value) =>  ValidateProperty(value, nameof(Name));

        [ObservableProperty]
        public string? infoCard;

        [ObservableProperty]
        private string? email;

        [ObservableProperty]
        private string? phone;

        [ObservableProperty]
        private BaseStatusEnum lockFlag;

        [ObservableProperty]
        private string? remark;


        private FormSubmitEventHandler<SysUser> SubmitEvent;

        public UserEditorViewModel(SysUser sysUser, FormSubmitEventHandler<SysUser> submitEvent) {
            this.SubmitEvent = submitEvent;

            if (sysUser.UserId.HasValue) {
                this.userId = sysUser.UserId;
                editModel = false;
            }
            
            this.Username = sysUser.Username;
            this.InfoCard = sysUser.InfoCard;
            this.Name = sysUser.Name;
            this.Email = sysUser.Email;
            this.Phone = sysUser.Phone;
            this.LockFlag = sysUser.LockFlag;
            this.Remark = sysUser.Remark;
        }

        [RelayCommand]
        private void submit() {

            ValidateAllProperties();
            if (HasErrors) return;
            SysUser entity = new()
            {
                UserId = userId,
                Username = Username,
                Name = Name,
                Phone = Phone,
                InfoCard = InfoCard,
                Email = Email,
                Remark = Remark,
                LockFlag = LockFlag
            };

            SubmitEvent(entity);
           
        }

    }
}
