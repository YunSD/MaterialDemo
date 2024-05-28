using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockMaterialStatementView : INavigableView<StockMaterialStatementViewModel>
    {
        public StockMaterialStatementViewModel ViewModel { get; }

        public StockMaterialStatementView(StockMaterialStatementViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
