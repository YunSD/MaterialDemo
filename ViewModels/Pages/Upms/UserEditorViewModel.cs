using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class UserEditorViewModel:ObservableValidator
    {
        
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

        [Required(ErrorMessage = "该字段不能为空")]
        [EmailAddress(ErrorMessage = "该字段只能为邮箱格式")]
        [ObservableProperty]
        private string? email;

        partial void OnEmailChanged(string? value) => ValidateProperty(value, nameof(Email));

        [Required(ErrorMessage = "该字段不能为空")]
        [Phone(ErrorMessage = "该字段只能为电话格式")]
        [ObservableProperty]
        private string? phone;
        partial void OnPhoneChanged(string? value) => ValidateProperty(value, nameof(Phone));

        [ObservableProperty]
        private string? lockFlag = "1";


        public delegate bool SaveEventHandler(object sender, DialogOpenedEventArgs eventArgs);

        private FormSubmitEventHandler<SysUser> SubmitEvent;

        public UserEditorViewModel(SysUser sysUser, FormSubmitEventHandler<SysUser> submitEvent) { 
            this.userId = sysUser.UserId;
            this.username = sysUser.Username;
            this.name = sysUser.Name;
            this.email = sysUser.Email;
            this.phone = sysUser.Phone;
            if (!String.IsNullOrEmpty(sysUser.LockFlag)) this.lockFlag = sysUser.LockFlag;
            this.SubmitEvent = submitEvent;
        }

        [RelayCommand]
        private void submit() {
            ValidateAllProperties();
            if (HasErrors) return;

            SysUser sysUser = new SysUser();
            sysUser.UserId = userId;
            sysUser.Username = Username;
            sysUser.Name = Name;
            sysUser.Phone = Phone;
            sysUser.Email = Email;
            sysUser.LockFlag = LockFlag;

            SubmitEvent(sysUser);
        }

    }
}
