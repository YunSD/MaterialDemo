﻿using HandyControl.Data;
using log4net;
using MaterialDemo.Config.Extensions;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Config.UnitOfWork.Collections;
using MaterialDemo.Controls;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using MaterialDemo.Views.Pages.Upms;
using System.Linq.Expressions;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class RoleViewModel : PageViewModelBase<SysRole>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(RoleViewModel));

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysRole> repository;

        public RoleViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<SysRole>();
        }


        #region View Field

        [ObservableProperty]
        private string? _SearchName;

        [ObservableProperty]
        private string? _SearchCode;



        [RelayCommand]
        private void OnSearch()
        {
            Expression<Func<SysRole, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchName)) { expression = expression.MergeAnd(expression, exp => exp.Name != null && exp.Name.Contains(SearchName)); }
            if (!string.IsNullOrWhiteSpace(SearchCode)) { expression = expression.MergeAnd(expression, exp => exp.Code != null && exp.Code.Contains(SearchCode)); }

            Func<IQueryable<SysRole>, IOrderedQueryable<SysRole>> orderBy = q => q.OrderBy(u => u.CreateTime);

            IPagedList<SysRole> pageList = repository.GetPagedList(expression, orderBy: orderBy, pageIndex: this.PageIndex, pageSize: PageSize);
            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            SearchName = null;
            SearchCode = null;
            this.OnSearch();
        }

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
        [RelayCommand]
        private async Task OpenEditForm(SysRole? entity)
        {
            SysRole data = new SysRole();
            if (entity != null) data = entity;
            RoleEditorViewModel editorViewModel = new RoleEditorViewModel(data, SubmitEventHandler);
            var form = new RoleEditorView(editorViewModel);
            var result = await DialogHost.Show(form, BaseConstant.BaseDialog);
        }


        /// <summary>
        /// form save command
        /// </summary>
        private void SubmitEventHandler(SysRole entity)
        {
            if (!entity.RoleId.HasValue)
            {
                entity.RoleId = SnowflakeIdWorker.Singleton.nextId();
                repository.Insert(entity);
            }
            else
            {
                repository.Update(entity);
            }
            _unitOfWork.SaveChanges();
            repository.ChangeEntityState(entity, Microsoft.EntityFrameworkCore.EntityState.Detached);
            this.OnSearch();
            DialogHost.Close(BaseConstant.BaseDialog);
        }


        /// <summary>
        ///  删除 command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task DelConfirm(SysRole entity)
        {
            if (!entity.RoleId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            this.rowId = entity.RoleId;
            var result = await DialogHost.Show(confirm, BaseConstant.BaseDialog, DeleteRowData);
        }

        // key
        private long? rowId;

        // reference method
        private void DeleteRowData(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "false")) return;
            if (rowId == null) return;

            // 删除角色数据
            IRepository<SysRoleMenu> rmRepository = _unitOfWork.GetRepository<SysRoleMenu>();
            List<SysRoleMenu> roleMenus = rmRepository.GetAll(predicate: m => m.RoleId == rowId).ToList();
            if (roleMenus.Any())
            {
                rmRepository.Delete(roleMenus);
            }

            repository.Delete(rowId);

            _unitOfWork.SaveChanges();

            // 刷新
            this.OnSearch();
        }

        [RelayCommand]
        private async Task Config(SysRole entity)
        {
            if (!entity.RoleId.HasValue) return;
            RoleMenuSelectViewModel model = new(entity, _unitOfWork);
            RoleMenuEditorView view = new RoleMenuEditorView(model);
            var result = await DialogHost.Show(view, BaseConstant.BaseDialog);
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
