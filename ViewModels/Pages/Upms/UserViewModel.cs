using CommunityToolkit.Mvvm.Messaging;
using HandyControl.Controls;
using HandyControl.Interactivity;
using HandyControl.Properties.Langs;
using HandyControl.Tools.Extension;
using log4net;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Config.UnitOfWork.Collections;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.Views.Pages.Login;
using MaterialDemo.Views.Pages.Upms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
            _unitOfWork = unitOfWork;
            sys_db = _unitOfWork.GetRepository<SysUser>();
        }


        #region View Field

        [ObservableProperty]
        private string? _username;

        [ObservableProperty]
        private string? _name;


        [RelayCommand]
        private void OnSearch() {
            Expression<Func<SysUser, bool>> pre = null;
            if(!String.IsNullOrEmpty(Username)) pre = p => p.Username != null && p.Username.Contains(Username);
            if(!String.IsNullOrEmpty(Name)) pre = p => p.Name != null && p.Name.Contains(Name);

            IPagedList<SysUser> pageList = sys_db.GetPagedList(predicate: pre, pageSize: PageSize);
            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.Username = null;
            this.Name = null;
            this.OnSearch();
        }

        #endregion

        #region From Command

        [RelayCommand]
        private async Task OpenFormWindow(object? _)
        {
            var form = new UserEditor();
            var result = await DialogHost.Show(form, SystemConstant.RootDialog,null, ClosingEventHandler, ClosedEventHandler);
            logger.Debug(result);
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        => Debug.WriteLine("You can intercept the closing event, and cancel here.");

        private void ClosedEventHandler(object sender, DialogClosedEventArgs eventArgs)
            => Debug.WriteLine("You can intercept the closed event here (1).");


        #endregion


        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            Task.Run(() =>
            {
                IPagedList<SysUser> pageList = sys_db.GetPagedList();
                base.RefreshPageInfo(pageList);
            });
        }
    }
}
