using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockExceptionViewPage : INavigableView<StockExceptionViewModel>
    {
        public StockExceptionViewModel ViewModel { get; }

        public StockExceptionViewPage(StockExceptionViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
