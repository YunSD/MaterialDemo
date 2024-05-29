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
        #endregion

        #region Fields
        private LoginViewPage LoginViewPage;
        private HomeViewPage HomeViewPage;
        #endregion

        public MainWindow(MainWindowViewModel viewModel, LoginViewPage loginView, HomeViewPage HomeView)
        {
            this.ViewModel = viewModel;
            this.LoginViewPage = loginView;
            this.HomeViewPage = HomeView;

            this.DataContext = this;

            InitializeComponent();

            // default
            MainFrame.Navigated += (sender, e) => { while (MainFrame.NavigationService.RemoveBackEntry() != null); };

            this.Navigate(LoginViewPage);

            // register message
            WeakReferenceMessenger.Default.Register<LoginCompletedRedirectionMessage>(this);
            WeakReferenceMessenger.Default.Register<LogoutMessage>(this);

            // 初始化 SnackbarPresenter
            SnackbarService.Singleton.SetSnackbarPresenter(SnackbarPresenter);
        }

        #region Window methods

        public void Navigate(Page page) {
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
            this.Navigate(HomeViewPage);
        }

        public void Receive(LogoutMessage message)
        {
            this.Navigate(LoginViewPage);
        }
    }

}