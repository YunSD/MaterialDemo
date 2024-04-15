using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class UserViewModel : PageViewModelBase<SysUser>, INavigationAware
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserViewModel(IUnitOfWork unitOfWork)
        {
            PageSize = 10;
            _unitOfWork = unitOfWork;
        }

        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {

            PageSize = 100;
            IRepository<SysUser> sys_db =  _unitOfWork.GetRepository<SysUser>();
            DataList = sys_db.GetAll().ToList();
        }
    }
}
