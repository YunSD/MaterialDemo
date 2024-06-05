using MaterialDemo.ViewModels.Pages.Upms;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Upms
{
    public partial class PersonViewPage : INavigableView<PersonViewModel>
    {
        public PersonViewModel ViewModel { get; }

        public PersonViewPage(PersonViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Password = Password.Password;
            ViewModel.RepeatPassword = RepeatPassword.Password;
            ViewModel.UpdatePasswordCommand.Execute(ViewModel);
        }
    }
}
