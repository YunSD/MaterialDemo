using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Home
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "首页",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.Login.LoginView)
            },
            
            new NavigationViewItem()
            {
                Content = "物料管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BoxMultipleSearch24 },
                TargetPageType = typeof(Views.Pages.Business.StockMaterialView),
            },
            new NavigationViewItem()
            {
                Content = "标签管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.TagMultiple24 },
                TargetPageType = typeof(Views.Pages.Business.ElectronicTagView),
            },
            new NavigationViewItem()
            {
                Content = "货位管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.AppFolder24 },
                TargetPageType = typeof(Views.Pages.Business.StockShelfView),
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "用户管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                TargetPageType = typeof(Views.Pages.Upms.UserView),
            },
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "Home", Tag = "tray_home" }
        };
    }
}
