using MaterialDemo.ViewModels.Pages.Upms;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Upms
{
    /// <summary>
    /// UserView.xaml 的交互逻辑
    /// </summary>
    public partial class UserView : INavigableView<UserViewModel>
    {
        public UserViewModel ViewModel { get; }

        public UserView(UserViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }
    }
}
