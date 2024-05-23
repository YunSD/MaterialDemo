using MaterialDemo.ViewModels.Pages.Home;
using MaterialDemo.ViewModels.Windows;
using System.Windows.Controls;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Base
{
    /// <summary>
    /// HomeView.xaml 的交互逻辑
    /// </summary>
    public partial class HomeView : Page, INavigationWindow
    {

        public MainWindowViewModel WindowViewModel { get; set; }
        public HomeViewModel NavigationItems { get; set; }

        public HomeView(IPageService pageService, INavigationService navigationService, MainWindowViewModel windowViewModel, HomeViewModel homeViewModel)
        {
            this.WindowViewModel = windowViewModel;
            this.NavigationItems = homeViewModel;
            DataContext = this;
            InitializeComponent();
            
            SetPageService(pageService);
            navigationService.SetNavigationControl(RootNavigation);

           
        }

        #region INavigationWindow methods

        public INavigationView GetNavigation() => RootNavigation;

        public bool Navigate(Type pageType) => RootNavigation.Navigate(pageType);

        public void SetPageService(IPageService pageService) => RootNavigation.SetPageService(pageService);

        public void ShowWindow() => throw new NotImplementedException();

        public void CloseWindow() => throw new NotImplementedException();

        public void SetServiceProvider(IServiceProvider serviceProvider) => RootNavigation.SetServiceProvider(serviceProvider);

        #endregion INavigationWindow methods

        private void NavigationToggle(object? sender, RoutedEventArgs? e)
        {
            RootNavigation.IsPaneOpen = !RootNavigation.IsPaneOpen;
        }
    }
}
