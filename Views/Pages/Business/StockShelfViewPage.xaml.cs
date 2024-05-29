using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockShelfViewPage : INavigableView<StockShelfViewModel>
    {
        public StockShelfViewModel ViewModel { get; }

        public StockShelfViewPage(StockShelfViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
