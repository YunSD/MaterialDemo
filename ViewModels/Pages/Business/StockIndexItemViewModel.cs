using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business.VObject;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class StockIndexItemViewModel : ObservableObject
    {
        
        [ObservableProperty]
        private string? _MaterialName;
        [ObservableProperty]
        private string? _MaterialCode;
        [ObservableProperty]
        private string? _MaterialModel;
        [ObservableProperty]
        private int? _Quantity;
        [ObservableProperty]
        private string? _CurrentQuantity;

        [ObservableProperty]
        private string? _MaterialImage;

        private readonly IUnitOfWork _unitOfWork;

        public StockIndexItemViewModel(IUnitOfWork unitOfWork, StockIndexItem item)
        {
            _unitOfWork = unitOfWork;

            MaterialName = item.StockMaterial?.Name;
            MaterialCode = item.StockMaterial?.Code;
            MaterialModel = item.StockMaterial?.Model;
            MaterialImage = item.StockMaterial?.Image;
            Quantity = item.Quantity;
        }


        [RelayCommand]
        private void CloseView()
        {
            if (!DialogHost.IsDialogOpen(BaseConstant.BaseDialog)) return;
            DialogHost.Close(BaseConstant.BaseDialog);
        }
    }
}
