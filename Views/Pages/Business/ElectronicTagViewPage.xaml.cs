using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class ElectronicTagViewPage : INavigableView<ElectronicTagViewModel>
    {
        public ElectronicTagViewModel ViewModel { get; }

        public ElectronicTagViewPage(ElectronicTagViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
