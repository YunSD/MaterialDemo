using MaterialDemo.Domain;
using MaterialDemo.Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class UserEditorViewModel:ObservableObject
    {

        [ObservableProperty]
        private SysUser _SysUser;


        public delegate bool SaveEventHandler(object sender, DialogOpenedEventArgs eventArgs);

        private FormSubmitEventHandler<SysUser> SubmitEvent;

        public UserEditorViewModel(SysUser sysUser, FormSubmitEventHandler<SysUser> submitEvent) { 
            this.SysUser = sysUser;
            this.SubmitEvent = submitEvent;
        }


        [RelayCommand]
        private void submit() {
            SubmitEvent.Invoke(SysUser);
        }

    }
}
