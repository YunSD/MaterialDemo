using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockShelfView : INavigableView<StockShelfViewModel>
    {
        public StockShelfViewModel ViewModel { get; }

        public StockShelfView(StockShelfViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
