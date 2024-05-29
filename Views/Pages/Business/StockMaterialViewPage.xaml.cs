using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockMaterialViewPage : INavigableView<StockMaterialViewModel>
    {
        public StockMaterialViewModel ViewModel { get; }

        public StockMaterialViewPage(StockMaterialViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
