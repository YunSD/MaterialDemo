using MaterialDemo.Config.Extensions;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business.VObject;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockIndexItemViewModel : ObservableObject, INavigationAware
    {
        [ObservableProperty]
        private string _MaterialName;
        [ObservableProperty]
        private string _MaterialCode;
        [ObservableProperty]
        private string _MaterialModel;
        [ObservableProperty]
        private string _Quantity;
        [ObservableProperty]
        private string _CurrentQuantity;

        private readonly IUnitOfWork _unitOfWork;

        public StockIndexItemViewModel(IUnitOfWork unitOfWork, StockIndexItem item)
        {
            _unitOfWork = unitOfWork;
        }


        private List<StockIndexItem> AllData = new();

        [ObservableProperty]
        private List<StockIndexItem> _Items = new();

        [ObservableProperty]
        private int _PageCount = 0;

        [ObservableProperty]
        private int _PageIndex = 1;

        [RelayCommand]
        private void ToNext() {
            if (PageIndex < PageCount) { 
                PageIndex = PageIndex + 1; 
                this.ChangePageData();
            }
        }

        [RelayCommand]
        private void ToPrevious()
        {
            if (PageIndex > 1) {
                PageIndex = PageIndex - 1;
                this.ChangePageData();
            }
        }

        private void ChangePageData()
        {
            Items = AllData.Skip(24 * (PageIndex - 1)).Take(Math.Min(24, AllData.Count - 24 * (PageIndex - 1))).ToList();
        }

        public void OnNavigatedTo()
        {
            List<StockShelf> shelves = repository.GetAll().ToList();

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


            var date = shelves.Select(ss => {
                StockIndexItem item = MapperUtil.Map<StockShelf, StockIndexItem>(ss);
                electronicTags.Where(tag => tag.TagId == item.TagId).GetFirstIfPresent(entity => item.ElectronicTag = entity);
                stockMaterials.Where(tag => tag.MaterialId == item.MaterialId).GetFirstIfPresent(entity => item.StockMaterial = entity);
                return item;
            }).ToList();

            AllData.AddRange(date);
            PageCount = (AllData.Count / 24) + 1;
            Items = AllData.Take(Math.Min(24, AllData.Count)).ToList();
        }

        public void OnNavigatedFrom()
        {
        }
    }
}
