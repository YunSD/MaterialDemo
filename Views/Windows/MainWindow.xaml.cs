using CommunityToolkit.Mvvm.Messaging;
using MaterialDemo.Security.Messages;
using MaterialDemo.ViewModels.Windows;
using MaterialDemo.Views.Pages.Login;
using System.Windows.Controls;

namespace MaterialDemo.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,IRecipient<LoginCompletedRedirectionMessage>
    {
        #region ViewModel
        public MainWindowViewModel ViewModel { get; }
        #endregion

        #region Fields
        private LoginView LoginViewPage;
        #endregion

        #region

        #endregion

        public MainWindow(MainWindowViewModel viewModel, LoginView loginView)
        {
            this.ViewModel = viewModel;
            this.LoginViewPage = loginView;

            this.DataContext = this;

            InitializeComponent();

            // default
            this.Navigate(LoginViewPage);
        }

        #region Window methods

        //public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Page page) => MainFrame.Navigate(page);

        //public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

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
            this.Navigate(null);
        }
    }

}