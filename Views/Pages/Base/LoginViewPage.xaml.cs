using log4net;
using MaterialDemo.ViewModels.Pages.Base;
using MaterialDemo.ViewModels.Windows;
using System.Diagnostics;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Base
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginViewPage : Page
    {

        #region ViewModel
        public MainWindowViewModel MainViewModel { get; }
        public LoginViewModel LoginViewModel { get; }
        #endregion

        private ILog logger = LogManager.GetLogger(nameof(LoginViewPage));

        public LoginViewPage(MainWindowViewModel mainViewModel, LoginViewModel loginViewModel)
        {
            this.MainViewModel = mainViewModel;
            this.LoginViewModel = loginViewModel;
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
