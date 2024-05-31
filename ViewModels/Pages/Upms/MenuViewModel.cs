using log4net;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Controls;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Upms.VObject;
using MaterialDemo.Views.Pages.Upms;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Upms
{
    public partial class MenuViewModel : PageViewModelBase<MenuTreeViewInfo>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(MenuViewModel));

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SysMenu> repository;

        public MenuViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<SysMenu>();
        }


        #region View Field

        [RelayCommand]
        private void OnSearch() {
            base.RefreshPageInfo(MenuTreeViewInfo.build(repository.GetAll().ToList()));
            //Expression<Func<SysUser, bool>> expression = ex => true;
            //if (!string.IsNullOrWhiteSpace(Username)) { expression = expression.MergeAnd(expression, exp => exp.Username != null && exp.Username.Contains(Username)); }
            //if (!string.IsNullOrWhiteSpace(Name)) { expression = expression.MergeAnd(expression, exp => exp.Name != null && exp.Name.Contains(Name)); }

            //Func<IQueryable<SysUser>, IOrderedQueryable<SysUser>> orderBy = q => q.OrderBy(u => u.CreateTime);

            //IPagedList<SysUser> pageList = sys_db.GetPagedList(expression, orderBy:orderBy, pageIndex: this.PageIndex, pageSize: PageSize);
            //base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            //this.Username = null;
            //this.Name = null;
            //this.OnSearch();
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
            UserEditorViewModel editorViewModel = new UserEditorViewModel(data, SubmitEventHandler);
            var form = new UserEditorView(editorViewModel);
            var result = await DialogHost.Show(form, BaseConstant.BaseDialog);
            logger.Debug(result);
        }


        /// <summary>
        /// form save command
        /// </summary>
        /// <param name="sysUser"></param>
        private void SubmitEventHandler(SysUser sysUser) {
            //if (!sysUser.UserId.HasValue)
            //{
            //    Expression<Func<SysUser, bool>> pre = p => p.Username == sysUser.Username;
            //    if (sys_db.Exists(pre))
            //    {
            //        SnackbarService.ShowError("用户登录名：" + sysUser.Username + " 不能重复");
            //        return;
            //    }
            //    sysUser.UserId = SnowflakeIdWorker.Singleton.nextId();
            //    sys_db.Insert(sysUser);
            //}
            //else {
            //    sys_db.Update(sysUser);
            //}
            //_unitOfWork.SaveChanges();
            //sys_db.ChangeEntityState(sysUser, Microsoft.EntityFrameworkCore.EntityState.Detached);
            //this.OnSearch();
            //DialogHost.Close(BaseConstant.BaseDialog);
        }


        /// <summary>
        ///  删除 command
        /// </summary>
        /// <param name="sys"></param>
        /// <returns></returns>
        [RelayCommand]
        private async Task DelConfirm(SysUser sys) {
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
            //if (Equals(eventArgs.Parameter, "false")) return;
            //if (rowId == null) return;
            //sys_db.Delete(rowId);
            //_unitOfWork.SaveChanges();

            //// 刷新
            //this.OnSearch();
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
