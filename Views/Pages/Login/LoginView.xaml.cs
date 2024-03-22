using MaterialDemo.ViewModels.Windows;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Login
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Page
    {

        #region ViewModel
        public MainWindowViewModel ViewModel { get; }
        #endregion

        public LoginView(MainWindowViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }


        // Used for DialogHost.DialogClosingAttached
        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine($"SAMPLE 2: Closing dialog with parameter: {eventArgs.Parameter ?? string.Empty}");

        private void Sample2_DialogHost_OnDialogClosed(object sender, DialogClosedEventArgs eventArgs)
            => Debug.WriteLine($"SAMPLE 2: Closed dialog with parameter: {eventArgs.Parameter ?? string.Empty}");
    }
}
