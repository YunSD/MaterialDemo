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

        private SysUser sysUser;

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

        //partial void OnEmailChanged(string? value) => ValidateProperty(value, nameof(Email));

        [ObservableProperty]
        private string? phone;
        //partial void OnPhoneChanged(string? value) => ValidateProperty(value, nameof(Phone));

        [ObservableProperty]
        private BaseStatusEnum lockFlag;

        [ObservableProperty]
        private string? remark;


        public delegate bool SaveEventHandler(object sender, DialogOpenedEventArgs eventArgs);

        private FormSubmitEventHandler<SysUser> SubmitEvent;

        public UserEditorViewModel(SysUser sysUser, FormSubmitEventHandler<SysUser> submitEvent) { 
            this.sysUser = sysUser;

            if (sysUser.UserId != null) {
                this.userId = sysUser.UserId;
                editModel = false;
            }
            
            this.username = sysUser.Username;
            this.infoCard = sysUser.InfoCard;
            this.name = sysUser.Name;
            this.email = sysUser.Email;
            this.phone = sysUser.Phone;

            this.lockFlag = sysUser.LockFlag;

            this.remark = sysUser.Remark;
            this.SubmitEvent = submitEvent;
        }

        [RelayCommand]
        private void submit() {

            ValidateAllProperties();
            if (HasErrors) return;

            sysUser.UserId = userId;
            sysUser.Username = Username;
            sysUser.Name = Name;
            sysUser.Phone = Phone;
            sysUser.InfoCard = InfoCard;
            sysUser.Email = Email;
            sysUser.Remark = Remark;
            sysUser.LockFlag = LockFlag;

            SubmitEvent(sysUser);

           
        }

    }
}
