using MaterialDemo.Config.Extensions;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business.VObject;
using MaterialDemo.Views.Pages.Business;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockIndexViewModel : ObservableObject, INavigationAware
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<StockShelf> repository;
        private readonly IRepository<ElectronicTag> tag_repository;
        private readonly IRepository<StockMaterial> material_repository;


        public StockIndexViewModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<StockShelf>();
            tag_repository = _unitOfWork.GetRepository<ElectronicTag>();
            material_repository = _unitOfWork.GetRepository<StockMaterial>();
        }


        private List<StockIndexItem> AllData = new();

        [ObservableProperty]
        private ObservableCollection<StockIndexItem> _Items = new();

        [ObservableProperty]
        private int _PageCount = 0;

        [ObservableProperty]
        private int _PageIndex = 1;

        [ObservableProperty]
        private bool _ETagConnectStatus = false;

        [ObservableProperty]
        private bool _ScaleConnectStatus = false;

        [RelayCommand]
        private void ToNext()
        {
            if (PageIndex < PageCount)
            {
                PageIndex = PageIndex + 1;
                this.ChangePageData();
            }
        }

        [RelayCommand]
        private void ToPrevious()
        {
            if (PageIndex > 1)
            {
                PageIndex = PageIndex - 1;
                this.ChangePageData();
            }
        }

        private void ChangePageData()
        {
            Items = new ObservableCollection<StockIndexItem>(AllData.Skip(24 * (PageIndex - 1)).Take(Math.Min(24, AllData.Count - 24 * (PageIndex - 1))).ToList());
        }

        [RelayCommand]
        public void ItemView(StockIndexItem? entity)
        {
            if (entity == null) return;
            StockIndexItemViewModel itemViewModel = new StockIndexItemViewModel(_unitOfWork, entity);
            var view = new StockIndexItemView(itemViewModel);
            DialogHost.Show(view, BaseConstant.BaseDialog);
        }

        public void OnNavigatedTo()
        {
            List<StockShelf> shelves = repository.GetAll().OrderBy(e=>e.Code).ToList();

            List<ElectronicTag> electronicTags = new();
            List<StockMaterial> stockMaterials = new();
            if (shelves.Any())
            {
                List<long?> tag_ids = shelves.ToList().Select(x => x.TagId).Where(x => x.HasValue).ToList();
                List<long?> material_ids = shelves.ToList().Select(x => x.MaterialId).Where(x => x.HasValue).ToList();
                if (tag_ids.Any())
                    electronicTags.AddRange(tag_repository.GetAll(predicate: pre => tag_ids.Contains(pre.TagId)).ToList());
                if (material_ids.Any())
                    stockMaterials.AddRange(material_repository.GetAll(predicate: pre => material_ids.Contains(pre.MaterialId)).ToList());
            }


            var date = shelves.Select(ss =>
            {
                StockIndexItem item = MapperUtil.Map<StockShelf, StockIndexItem>(ss);
                electronicTags.Where(tag => tag.TagId == item.TagId).GetFirstIfPresent(entity => item.ElectronicTag = entity);
                stockMaterials.Where(tag => tag.MaterialId == item.MaterialId).GetFirstIfPresent(entity => item.StockMaterial = entity);
                return item;
            }).ToList();

            AllData.AddRange(date);
            PageCount = (AllData.Count / 24) + 1;
            Items = new ObservableCollection<StockIndexItem>(AllData.Take(Math.Min(24, AllData.Count)).ToList());

            // 更新库存地址信息
            Task.Run(() => DataAcquisitionService.Singleton.loadBasicData());

            // register control
            DataAcquisitionService.Singleton.ItemChangeNotice += ItemChangeNotice;

        }

        public void ItemChangeNotice(object? sender, ItemChangeNotice notice)
        {
            StockIndexItem? item = Items.FirstOrDefault(x => x.ShelfId == notice.key);
            if (item == null || item.Quantity == notice.after) return;
            item.Quantity = notice.after;
        }

        public void OnNavigatedFrom()
        {
            DataAcquisitionService.Singleton.ItemChangeNotice -= ItemChangeNotice;
        }

    }
}
