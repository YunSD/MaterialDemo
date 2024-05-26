using AutoMapper;
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
using MaterialDemo.ViewModels.Pages.Business.VObject;
using MaterialDemo.Views.Pages.Business;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockShelfViewModel : PageViewModelBase<StockShelfViewInfo>, INavigationAware
    {

        private ILog logger = LogManager.GetLogger(nameof(StockShelfViewModel));

        private readonly IUnitOfWork _unitOfWork;

        private readonly IRepository<StockShelf> repository;
        private readonly IRepository<ElectronicTag> tag_repository;
        private readonly IRepository<StockMaterial> material_repository;

        private ElectronicTagViewModel TagViewModel { get; }
        private StockMaterialViewModel MaterialViewModel { get; }

        public StockShelfViewModel(IUnitOfWork unitOfWork, ElectronicTagViewModel tagViewModel, StockMaterialViewModel materialViewModel)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<StockShelf>();
            tag_repository = _unitOfWork.GetRepository<ElectronicTag>();
            material_repository = _unitOfWork.GetRepository<StockMaterial>();
            
            TagViewModel = tagViewModel;
            MaterialViewModel = materialViewModel;
        }


        #region View Field

        // 货位号
        [ObservableProperty]
        private string? _searchCode;

        // 货架号
        [ObservableProperty]
        private string? _searchShelvesCode;


        [RelayCommand]
        private void OnSearch() {

            Expression<Func<StockShelf, bool>> expression = ex => true;
            if (!string.IsNullOrWhiteSpace(SearchCode)) expression = expression.MergeAnd(expression, exp => exp.Code != null && exp.Code.Contains(SearchCode));
            if (!string.IsNullOrWhiteSpace(SearchShelvesCode)) expression = expression.MergeAnd(expression, exp => exp.ShelvesCode != null && exp.ShelvesCode.Contains(SearchShelvesCode));

            Func<IQueryable<StockShelf>, IOrderedQueryable<StockShelf>> orderBy = q => q.OrderBy(u => u.CreateTime);

            IPagedList<StockShelf> pageList = repository.GetPagedList(predicate: expression, orderBy:orderBy, pageIndex: this.PageIndex, pageSize: PageSize);


            List<ElectronicTag> electronicTags = new ();
            List<StockMaterial> stockMaterials = new ();
            if (pageList.Items.Any()) {
                List<long?> tag_ids = pageList.Items.ToList().Select(x => x.TagId).Where(x => x.HasValue).ToList();
                List<long?> material_ids = pageList.Items.ToList().Select(x => x.MaterialId).Where(x => x.HasValue).ToList();
                if(tag_ids.Any())
                    electronicTags.AddRange(tag_repository.GetAll(predicate: pre => tag_ids.Contains(pre.TagId)).ToList());
                if (material_ids.Any())
                    stockMaterials.AddRange(material_repository.GetAll(predicate: pre => material_ids.Contains(pre.MaterialId)).ToList());
            }


            var date = pageList.Items.Select(ss => {
                StockShelfViewInfo viewInfo = MapperUtil.Map<StockShelf,StockShelfViewInfo>(ss);
                electronicTags.Where(tag => tag.TagId == viewInfo.TagId).GetFirstIfPresent(entity => viewInfo.ElectronicTag = entity);
                stockMaterials.Where(tag => tag.MaterialId == viewInfo.MaterialId).GetFirstIfPresent(entity => viewInfo.StockMaterial = entity);
                return viewInfo;
            }).ToList();

            base.RefreshPageInfo(pageList, date);
        }

        [RelayCommand]
        private void OnRefresh()
        {
            this.SearchCode = null;
            this.SearchShelvesCode = null;
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

        /// <summary>s
        /// edit form command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OpenEditForm(StockShelfViewInfo? entity)
        {
            StockShelfViewInfo data = new();
            if (entity != null) data = entity;
            StockShelfEditorViewModel editorViewModel = new StockShelfEditorViewModel(data, SubmitEventHandler);
            var form = new StockShelfEditorView(editorViewModel, TagViewModel, MaterialViewModel);
            var result = await DialogHost.Show(form, BaseConstant.BaseDialog);
            logger.Debug(result);
        }


        /// <summary>
        /// form save command
        /// </summary>
        private void SubmitEventHandler(StockShelf entity) {

            Expression<Func<StockShelf, bool>> pre = p => p.Code == entity.Code && p.TagId != entity.TagId;
            
            if (repository.Exists(pre))
            {
                SnackbarService.ShowError("货位编号：" + entity.Code + " 不能重复");
                return;
            }

            pre = p => p.TagId != null && p.TagId == entity.TagId;

            if (repository.Exists(pre))
            {
                SnackbarService.ShowError("此标签已被占用");
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
            DialogHost.Close(BaseConstant.BaseDialog);
        }


        /// <summary>
        ///  删除 command
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task DelConfirm(StockShelf entity) {
            if (!entity.TagId.HasValue) return;
            var confirm = new ConfirmDialog("确认删除？");
            this.rowId = entity.TagId;
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
