using MaterialDemo.Config.Security;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Security;
using MaterialDemo.Utils;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Base
{
    public partial class HomeViewModel : ObservableObject
    {

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new();
        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new();
        //[ObservableProperty]
        //private ObservableCollection<object> _menuItems = new()
        //{
        //    new NavigationViewItem()
        //    {
        //        Content = "首页",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
        //        TargetPageType = typeof(Views.Pages.Base.LoginViewPage)
        //    },
        //    new NavigationViewItem() {
        //        Content = "首页2",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
        //        TargetPageType = typeof(Views.Pages.Base.LoginViewPage)
        //    },
        //    new NavigationViewItem()
        //    {
        //        Content = "物料管理",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.BoxMultipleSearch24 },
        //        TargetPageType = typeof(Views.Pages.Business.StockMaterialViewPage),
        //    },
        //    new NavigationViewItem()
        //    {
        //        Content = "标签管理",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.TagMultiple24 },
        //        TargetPageType = typeof(Views.Pages.Business.ElectronicTagViewPage),
        //    },
        //    new NavigationViewItem()
        //    {
        //        Content = "货位管理",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.AppFolder24 },
        //        TargetPageType = typeof(Views.Pages.Business.StockShelfViewPage),
        //    },
        //    new NavigationViewItem()
        //    {
        //        Content = "存取日志",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable24 },
        //        TargetPageType = typeof(Views.Pages.Business.StockMaterialStatementViewPage),
        //    },
        //    new NavigationViewItem()
        //    {
        //        Content = "存取日志2",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable24 },
        //        TargetPageType = typeof(Views.Pages.Base.FolderViewPage),
        //    },
        //    new NavigationViewItem()
        //    {
        //        Content = "存取日志2",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable24 },
        //        TargetPageType = typeof(Views.Pages.Upms.MenuViewPage),
        //    },new NavigationViewItem()
        //    {
        //        Content = "角色管理",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.DocumentTable24 },
        //        TargetPageType = typeof(Views.Pages.Upms.RoleViewPage),
        //    }
        //};

        //[ObservableProperty]
        //private ObservableCollection<object> _footerMenuItems = new()
        //{
        //    new NavigationViewItem()
        //    {
        //        Content = "用户管理",
        //        Icon = new SymbolIcon { Symbol = SymbolRegular.Person24 },
        //        TargetPageType = typeof(Views.Pages.Upms.UserViewPage),
        //    },
        //};

        public HomeViewModel(){
            SecurityUser? user = SecurityContext.Singleton.getUserInfo();
            if (user != null && user.menus.Any()) { 

                Dictionary<long, NavigationViewItem> navigationMaps = new ();

                List<NavigationViewItem> topItem = new ();
                List<NavigationViewItem> footerItem = new ();
                
                user.menus.OrderBy(menu=>menu.Seq).ToList().ForEach(menu =>
                {
                    string? name = menu.Name;
                    if (name == null) name = "NULL";
                    NavigationViewItem item = new NavigationViewItem(name, BasePageUtil.ParseSymbolIcon(menu.Icon), BasePageUtil.ParseClassType(menu.Router));
                    navigationMaps.Add((long)menu.MenuId, item);
                    if (menu.isRoot() && MenuPositionEnum.TOP == menu.Position) topItem.Add(item); 
                    if (menu.isRoot() && MenuPositionEnum.BOTTOM == menu.Position) footerItem.Add(item); 
                });

                user.menus.ForEach(menu =>
                {
                    if (menu.isRoot()) return;
                    NavigationViewItem parentItem = navigationMaps[(long)(menu.ParentId)];
                    NavigationViewItem currentItem = navigationMaps[(long)(menu.MenuId)];
                    if(parentItem!=null && currentItem != null) parentItem.MenuItems.Add(currentItem);
                });

                topItem.ForEach(menu => MenuItems.Add(menu));
                footerItem.ForEach(menu => FooterMenuItems.Add(menu));
            }
        }

    }
}
