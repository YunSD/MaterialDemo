using log4net;
using MahApps.Metro.Controls;
using MaterialDemo.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages;
using MaterialDemo.ViewModels.Windows;
using MaterialDesignThemes.Wpf;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media;

namespace MaterialDemo.Views.Pages.Login
{
    /// <summary>
    /// LoginView.xaml 的交互逻辑
    /// </summary>
    public partial class LoginView : Page
    {

        #region ViewModel
        public MainWindowViewModel MainViewModel { get; }
        public LoginViewModel LoginViewModel { get; }
        #endregion

        private ILog logger = LogManager.GetLogger(nameof(LoginView));

        public LoginView(MainWindowViewModel mainViewModel, LoginViewModel loginViewModel)
        {
            this.MainViewModel = mainViewModel;
            this.LoginViewModel = loginViewModel;
            this.DataContext = this;
            InitializeComponent();
            logger.Error("发生什么事了，发生什么事了，发生什么事了，发生什么事了");
        }


        // Used for DialogHost.DialogClosingAttached
        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
            => Debug.WriteLine($"SAMPLE 2: Closing dialog with parameter: {eventArgs.Parameter ?? string.Empty}");

        private void Sample2_DialogHost_OnDialogClosed(object sender, DialogClosedEventArgs eventArgs)
            => Debug.WriteLine($"SAMPLE 2: Closed dialog with parameter: {eventArgs.Parameter ?? string.Empty}");


        [RelayCommand]
        public void Submit(Object param)
        {
            DialogHost.OpenDialogCommand.Execute(param, this);
            if (this.DataContext != null)
            { 
                ((dynamic)this.DataContext).LoginViewModel.password = password.SecurePassword; 
            }
            //DialogHost.CloseDialogCommand.Execute(param, this);
        }
    }
}
