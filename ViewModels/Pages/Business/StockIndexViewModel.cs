using MaterialDemo.Config.Extensions;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business.VObject;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockIndexViewModel: ObservableObject, INavigationAware
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<StockShelf> repository;
        private readonly IRepository<ElectronicTag> tag_repository;
        private readonly IRepository<StockMaterial> material_repository;


        public StockIndexViewModel(IUnitOfWork unitOfWork )
        {
            _unitOfWork = unitOfWork;
            repository = _unitOfWork.GetRepository<StockShelf>();
            tag_repository = _unitOfWork.GetRepository<ElectronicTag>();
            material_repository = _unitOfWork.GetRepository<StockMaterial>();
        }


        [ObservableProperty]
        List<StockIndexItem> _Items = new();

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

            List<StockIndexItem> indexItems = new List<StockIndexItem>();
            for (int i = 0; i < 14; i++) {
                foreach (var item in date)
                {
                    indexItems.Add(item);
                }
            }
            Items = indexItems;

        }

        public void OnNavigatedFrom()
        {
        }
    }
}
