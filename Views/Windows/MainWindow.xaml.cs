namespace MaterialDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Window methods

        //public INavigationView GetNavigation() => RootNavigation;

        //public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

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

        private void username_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            usernameBox.Focus();
        }

        private void usernameBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(usernameBox.Text))
            {
                username.Visibility = Visibility.Collapsed;
            } else {
                username.Visibility = Visibility.Visible;
            }
        }

        private void password_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            passwordBox.Focus();
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(passwordBox.Password))
            {
                password.Visibility = Visibility.Collapsed;
            }
            else
            {
                password.Visibility = Visibility.Visible;
            }
        }
    }

}