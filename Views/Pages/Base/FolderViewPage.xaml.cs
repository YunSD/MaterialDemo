using MaterialDemo.ViewModels.Pages.Base;
using MaterialDemo.ViewModels.Pages.Upms;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Base
{
    public partial class FolderViewPage : INavigableView<FolderViewModel>
    {
        public FolderViewModel ViewModel { get; }

        public FolderViewPage(FolderViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
