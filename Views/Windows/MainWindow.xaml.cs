using CommunityToolkit.Mvvm.Messaging;
using MaterialDemo.Config.Security.Messages;
using MaterialDemo.ViewModels.Windows;
using MaterialDemo.Views.Pages.Base;
using System.Windows.Controls;
using Wpf.Ui;

namespace MaterialDemo.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IRecipient<LoginCompletedRedirectionMessage>, IRecipient<LogoutMessage>
    {
        #region ViewModel
        public MainWindowViewModel ViewModel { get; }
        public IServiceProvider ServiceProvider { get; }
        #endregion

        public MainWindow(MainWindowViewModel viewModel, IServiceProvider serviceProvider, LoginViewPage loginViewPage)
        {
            this.ViewModel = viewModel;
            this.ServiceProvider = serviceProvider;

            this.DataContext = this;

            InitializeComponent();

            this.Navigate(loginViewPage);

            // default
            MainFrame.Navigated += (sender, e) => { while (MainFrame.NavigationService.RemoveBackEntry() != null) ; };

            // register message
            WeakReferenceMessenger.Default.Register<LoginCompletedRedirectionMessage>(this);
            WeakReferenceMessenger.Default.Register<LogoutMessage>(this);

            // 初始化 SnackbarPresenter
            SnackbarService.Singleton.SetSnackbarPresenter(SnackbarPresenter);
        }

        #region Window methods

        public void Navigate(Page page)
        {
            MainFrame.Navigate(page);
        }


        public void ShowWindow() => base.Show();

        public void CloseWindow() => base.Close();

        #endregion Window methods


        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Make sure that closing this window will begin the process of closing the application.
            Application.Current.Shutdown();
        }

        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }

        public void Receive(LoginCompletedRedirectionMessage message)
        {
            this.Navigate(ServiceProvider.GetService(typeof(HomeViewPage)) as HomeViewPage);
        }

        public void Receive(LogoutMessage message)
        {
            this.Navigate(ServiceProvider.GetService(typeof(LoginViewPage)) as LoginViewPage);
        }
    }

}