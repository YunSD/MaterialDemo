using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using log4net;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Views.Pages.Login;
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

        private ILog logger = LogManager.GetLogger(nameof(UserViewModel));

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysUser> sys_db;

        public UserViewModel(IUnitOfWork unitOfWork)
        {
            PageSize = 10;
            _unitOfWork = unitOfWork;
            sys_db = _unitOfWork.GetRepository<SysUser>();
        }

        public void OnNavigatedFrom()
        {
            logger.Debug("dasdasdasdsadad");
        }

        public void OnNavigatedTo()
        {
            Task.Run(() =>
            {
                DataList = sys_db.GetAll().ToList();
            });
        }
    }
}
