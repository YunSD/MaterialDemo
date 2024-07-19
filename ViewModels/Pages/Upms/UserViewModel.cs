﻿using HandyControl.Data;
using log4net;
using MaterialDemo.Config.Extensions;
using MaterialDemo.Config.Security.Messages;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Config.UnitOfWork.Collections;
using MaterialDemo.Controls;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using MaterialDemo.Views.Pages.Upms;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Wpf.Ui;
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
        private void OnSearch()
        {

            Expression<Func<SysUser, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(Username)) { expression = expression.MergeAnd(expression, exp => exp.Username != null && exp.Username.Contains(Username)); }
            if (!string.IsNullOrWhiteSpace(Name)) { expression = expression.MergeAnd(expression, exp => exp.Name != null && exp.Name.Contains(Name)); }

            Func<IQueryable<SysUser>, IOrderedQueryable<SysUser>> orderBy = q => q.OrderBy(u => u.CreateTime);

            IPagedList<SysUser> pageList = sys_db.GetPagedList(expression, orderBy: orderBy, pageIndex: this.PageIndex, pageSize: PageSize);
            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.Username = null;
            this.Name = null;
            this.OnSearch();
        }

        /// <summary>
        ///     页码改变
        /// </summary>
        [RelayCommand]
        private void PageUpdated(FunctionEventArgs<int> info)
        {
            this.PageIndex = info.Info - 1;
            this.OnSearch();
        }


        #endregion

        #region From Command

        /// <summary>
        /// edit form command
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenEditForm(SysUser? user)
        {
            SysUser data = new SysUser();
            if (user != null) data = user;
            IRepository<SysRole> roleRepository = _unitOfWork.GetRepository<SysRole>();
            UserEditorViewModel editorViewModel = new UserEditorViewModel(data, roleRepository.GetAll().AsNoTracking().ToList(), SubmitEventHandler);
            var form = new UserEditorView(editorViewModel);
            var result = await DialogHost.Show(form, BaseConstant.BaseDialog);
            logger.Debug(result);
        }


        /// <summary>
        /// form save command
        /// </summary>
        /// <param name="sysUser"></param>
        private void SubmitEventHandler(SysUser sysUser)
        {
            if (!sysUser.UserId.HasValue)
            {
                Expression<Func<SysUser, bool>> pre = p => p.Username == sysUser.Username;
                if (sys_db.Exists(pre))
                {
                    SnackbarService.ShowError("用户登录名：" + sysUser.Username + " 不能重复");
                    return;
                }
                sysUser.UserId = SnowflakeIdWorker.Singleton.nextId();
                sys_db.Insert(sysUser);
            }
            else
            {
                sys_db.Update(sysUser);
            }
            _unitOfWork.SaveChanges();
            sys_db.ChangeEntityState(sysUser, Microsoft.EntityFrameworkCore.EntityState.Detached);
            this.OnSearch();
            DialogHost.Close(BaseConstant.BaseDialog);
        }


        /// <summary>
        ///  删除 command
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task DelConfirm(SysUser sys)
        {
            if (!sys.UserId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            this.rowId = sys.UserId;
            var result = await DialogHost.Show(confirm, BaseConstant.BaseDialog, DeleteRowData);
        }

        // key
        private long? rowId;

        // reference method
        private void DeleteRowData(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "false")) return;
            if (rowId == null) return;
            sys_db.Delete(rowId);
            _unitOfWork.SaveChanges();

            // 刷新
            this.OnSearch();
        }

        [RelayCommand]
        private void ResetPassword(SysUser user)
        {
            user.Password = SecurityUtil.Encrypt("123456");
            sys_db.Update(user);
            _unitOfWork.SaveChanges();
            SnackbarService.ShowSuccess("用户密码重置成功。");
            sys_db.ChangeEntityState(user, Microsoft.EntityFrameworkCore.EntityState.Detached);
        }

        #endregion


        public void OnNavigatedFrom()
        {
        }

        public void OnNavigatedTo()
        {
            Task.Run(() =>
            {
                this.OnSearch();
            });
        }
    }
}
