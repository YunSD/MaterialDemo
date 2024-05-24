using HandyControl.Data;
using Linq.PredicateBuilder;
using log4net;
using MaterialDemo.Config.Extensions;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Config.UnitOfWork.Collections;
using MaterialDemo.Controls;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using MaterialDemo.Views.Pages.Business;
using System.Linq.Expressions;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class ElectronicTagViewModel : PageViewModelBase<ElectronicTag>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(ElectronicTagViewModel));

        private readonly IUnitOfWork _unitOfWork;

        private readonly IRepository<ElectronicTag> repository;

        public ElectronicTagViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<ElectronicTag>();
        }


        #region View Field


        [ObservableProperty]
        private string? _searchCode;


        [RelayCommand]
        private void OnSearch() {

            Expression<Func<ElectronicTag, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchCode)) { expression = expression.MergeAnd(expression, exp => exp.Code != null && exp.Code.Contains(SearchCode)); }

            Func<IQueryable<ElectronicTag>, IOrderedQueryable<ElectronicTag>> orderBy = q => q.OrderBy(u => u.CreateTime);

            IPagedList<ElectronicTag> pageList = repository.GetPagedList(predicate: expression, orderBy:orderBy, pageIndex: this.PageIndex, pageSize: PageSize);
            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.SearchCode = null;
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
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenEditForm(ElectronicTag? entity)
        {
            ElectronicTag data = new ElectronicTag();
            if (entity != null) data = entity;
            ElectronicTagEditorViewModel editorViewModel = new ElectronicTagEditorViewModel(data, SubmitEventHandler);
            var form = new ElectronicTagEditorView(editorViewModel);
            var result = await DialogHost.Show(form, BaseConstant.RootDialog);
            logger.Debug(result);
        }


        /// <summary>
        /// form save command
        /// </summary>
        private void SubmitEventHandler(ElectronicTag entity) {

            Expression<Func<ElectronicTag, bool>> pre = p => p.Code == entity.Code && p.TagId != entity.TagId;
            
            if (repository.Exists(pre))
            {
                SnackbarService.ShowError("标签编码：" + entity.Code + " 不能重复");
                return;
            }

            if (!entity.TagId.HasValue)
            {
                entity.TagId = SnowflakeIdWorker.Singleton.nextId();
                repository.Insert(entity);
            }
            else {
                repository.Update(entity);
            }

            _unitOfWork.SaveChanges();
            repository.ChangeEntityState(entity, Microsoft.EntityFrameworkCore.EntityState.Detached);
            this.OnSearch();
            DialogHost.Close(BaseConstant.RootDialog);
        }


        /// <summary>
        ///  删除 command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task DelConfirm(ElectronicTag entity) {
            if (!entity.TagId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            this.rowId = entity.TagId;
            var result = await DialogHost.Show(confirm, BaseConstant.RootDialog, DeleteRowData);
        }

        // key
        private long? rowId;

        // reference method
        private void DeleteRowData(object sender, DialogClosingEventArgs eventArgs)
        {
            if (Equals(eventArgs.Parameter, "false")) return;
            if (rowId == null) return;
            repository.Delete(rowId);
            _unitOfWork.SaveChanges();

            // 刷新
            this.OnSearch();
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
