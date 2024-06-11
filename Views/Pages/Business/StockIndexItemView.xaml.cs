using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    /// <summary>
    /// StockIndex.xaml 的交互逻辑
    /// </summary>
    public partial class StockIndexItemView
    {

        public StockIndexItemViewModel ViewModel { get; }

        public StockIndexItemView(StockIndexItemViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
