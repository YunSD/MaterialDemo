using HandyControl.Data;
using Linq.PredicateBuilder;
using log4net;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Config.UnitOfWork.Collections;
using MaterialDemo.Controls;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.Views.Pages.Business;
using System.Linq.Expressions;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockMaterialViewModel : PageViewModelBase<StockMaterial>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(StockMaterialViewModel));

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<StockMaterial> repository;

        public StockMaterialViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<StockMaterial>();
        }


        #region View Field

        [ObservableProperty]
        private string? _searchName;

        [ObservableProperty]
        private string? _searchCode;


        [RelayCommand]
        private void OnSearch() {
            IQueryable<StockMaterial> Persons = repository.GetAll();
            Expression<Func<StockMaterial, bool>> expression = (Expression<Func<StockMaterial, bool>>)Persons.Build(e => e.Equals(x => x.Code, "11")).Expression;
            Expression<Func<StockMaterial, bool>> pre = p => p.Name != null && p.Name.Contains(SearchName);
            if (!String.IsNullOrEmpty(SearchName)) pre = p => p.Name != null && p.Name.Contains(SearchName);
            if(!String.IsNullOrEmpty(SearchCode)) pre = p => p.Name != null && p.Name.Contains(SearchCode);
            

            Func<IQueryable<StockMaterial>, IOrderedQueryable<StockMaterial>> orderBy = q => q.OrderBy(u => u.CreateTime);

            IPagedList<StockMaterial> pageList = repository.GetPagedList(predicate: expression, orderBy:orderBy, pageIndex: this.PageIndex, pageSize: PageSize);
            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.SearchName = null;
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
        private async Task OpenEditForm(StockMaterial? entity)
        {
            StockMaterial data = new StockMaterial();
            if (entity != null) data = entity;
            StockMaterialEditorViewModel editorViewModel = new StockMaterialEditorViewModel(data, SubmitEventHandler);
            var form = new StockMaterialEditorView(editorViewModel);
            var result = await DialogHost.Show(form, BaseConstant.RootDialog);
            logger.Debug(result);
        }


        /// <summary>
        /// form save command
        /// </summary>
        private void SubmitEventHandler(StockMaterial entity) {

            Expression<Func<StockMaterial, bool>> pre = p => p.Code == entity.Code && p.MaterialId != entity.MaterialId;
            
            if (repository.Exists(pre))
            {
                SnackbarService.ShowError("物料编码：" + entity.Code + " 不能重复");
                return;
            }

            if (!entity.MaterialId.HasValue)
            {
                entity.MaterialId = SnowflakeIdWorker.Singleton.nextId();
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
        private async Task DelConfirm(StockMaterial entity) {
            if (!entity.MaterialId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            this.rowId = entity.MaterialId;
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
