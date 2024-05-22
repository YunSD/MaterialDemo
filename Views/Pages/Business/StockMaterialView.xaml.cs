using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockMaterialView : INavigableView<StockMaterialViewModel>
    {
        public StockMaterialViewModel ViewModel { get; }

        public StockMaterialView(StockMaterialViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
