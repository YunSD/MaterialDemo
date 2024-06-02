using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Base
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
                TargetPageType = typeof(Views.Pages.Base.LoginViewPage)
            },
            
            new NavigationViewItem()
            {
                Content = "物料管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.BoxMultipleSearch24 },
                TargetPageType = typeof(Views.Pages.Business.StockMaterialViewPage),
            },
            new NavigationViewItem()
            {
                Content = "标签管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.TagMultiple24 },
                TargetPageType = typeof(Views.Pages.Business.ElectronicTagViewPage),
            },
            new NavigationViewItem()
            {
                Content = "货位管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.AppFolder24 },
                TargetPageType = typeof(Views.Pages.Business.StockShelfViewPage),
            },
            new NavigationViewItem()
            {
                Content = "存取日志",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable24 },
                TargetPageType = typeof(Views.Pages.Business.StockMaterialStatementViewPage),
            },
            new NavigationViewItem()
            {
                Content = "存取日志2",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable24 },
                TargetPageType = typeof(Views.Pages.Base.FolderViewPage),
            },
            new NavigationViewItem()
            {
                Content = "存取日志2",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable24 },
                TargetPageType = typeof(Views.Pages.Upms.MenuViewPage),
            },new NavigationViewItem()
            {
                Content = "角色管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable24 },
                TargetPageType = typeof(Views.Pages.Upms.RoleViewPage),
            }
        };

        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "用户管理",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
                TargetPageType = typeof(Views.Pages.Upms.UserViewPage),
            },
        };
    }
}
