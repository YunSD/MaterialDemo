using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockMaterialStatementViewPage : INavigableView<StockMaterialStatementViewModel>
    {
        public StockMaterialStatementViewModel ViewModel { get; }

        public StockMaterialStatementViewPage(StockMaterialStatementViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
