using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Models.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace MaterialDemo.ViewModels.Pages
{
    public partial class LoginViewModel:ObservableObject
    {

        private IUnitOfWork _unitOfWork;

        [ObservableProperty]
        public string? username;

        public SecureString? password { private get; set; }

        public LoginViewModel(IUnitOfWork unitOfWork) {
            this._unitOfWork = unitOfWork;
        }

        //[RelayCommand]
        //public void LoginMethod() {
        //    if (this.DataContext != null)
        //    { ((dynamic)this.DataContext).SecurePassword = ((PasswordBox)sender).SecurePassword; }

        //    var repository = _unitOfWork.GetRepository<SysUser>();
        //    List<SysUser> users = _unitOfWork.GetRepository<SysUser>().GetAll().ToList();
        //    Console.WriteLine("1");
            
        //}

    }
}
