using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    /// <summary>
    /// ScaleConfigViewPage.xaml 的交互逻辑
    /// </summary>
    public partial class ScaleConfigViewPage : INavigableView<ScaleConfigViewModel>
    {
        public ScaleConfigViewModel ViewModel { get; }

        public ScaleConfigViewPage(ScaleConfigViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
