using HandyControl.Data;
using log4net;
using MaterialDemo.Config.Extensions;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Config.UnitOfWork.Collections;
using MaterialDemo.Controls;
using MaterialDemo.Domain.Models;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using System.Linq.Expressions;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockMaterialStatementViewModel : PageViewModelBase<StockMaterialStatement>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(StockMaterialStatementViewModel));

        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<StockMaterialStatement> repository;

        public StockMaterialStatementViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<StockMaterialStatement>();
        }


        #region View Field

        [ObservableProperty]
        private string? _searchMaterialName;

        [ObservableProperty]
        private string? _searchMaterialCode;

        [ObservableProperty]
        private DateTime? _searchStartDate;

        [ObservableProperty]
        private DateTime? _searchEndDate;


        [RelayCommand]
        private void OnSearch()
        {

            Expression<Func<StockMaterialStatement, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchMaterialName)) { expression = expression.MergeAnd(expression, exp => exp.MaterialName != null && exp.MaterialName.Contains(SearchMaterialName)); }
            if (!string.IsNullOrWhiteSpace(SearchMaterialCode)) { expression = expression.MergeAnd(expression, exp => exp.MaterialCode != null && exp.MaterialCode.Contains(SearchMaterialCode)); }
            if (SearchStartDate != null) { expression = expression.MergeAnd(expression, exp => exp.CreateTime != null && exp.CreateTime >= SearchStartDate); }
            if (SearchEndDate != null) { expression = expression.MergeAnd(expression, exp => exp.CreateTime != null && exp.CreateTime <= SearchEndDate.Value.AddDays(1)); }


            Func<IQueryable<StockMaterialStatement>, IOrderedQueryable<StockMaterialStatement>> orderBy = q => q.OrderBy(u => u.CreateTime);

            IPagedList<StockMaterialStatement> pageList = repository.GetPagedList(predicate: expression, orderBy: orderBy, pageIndex: this.PageIndex, pageSize: PageSize);
            base.RefreshPageInfo(pageList);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.SearchMaterialName = null;
            this.SearchMaterialCode = null;
            this.SearchStartDate = null;
            this.SearchEndDate = null;
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
        ///  删除 command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task DelConfirm(StockMaterialStatement entity)
        {
            if (!entity.StatementId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            this.rowId = entity.StatementId;
            var result = await DialogHost.Show(confirm, BaseConstant.BaseDialog, DeleteRowData);
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
