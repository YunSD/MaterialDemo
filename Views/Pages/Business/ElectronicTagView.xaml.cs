using MaterialDemo.ViewModels.Pages.Business;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class ElectronicTagView : INavigableView<ElectronicTagViewModel>
    {
        public ElectronicTagViewModel ViewModel { get; }

        public ElectronicTagView(ElectronicTagViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
