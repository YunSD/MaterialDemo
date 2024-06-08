using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    /// <summary>
    /// StockIndex.xaml 的交互逻辑
    /// </summary>
    public partial class StockIndex : INavigableView<StockIndexViewModel>
    {
        public StockIndexViewModel ViewModel { get; }

        public StockIndex(StockIndexViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
