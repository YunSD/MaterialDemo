using CommunityToolkit.Mvvm.Messaging;
using MaterialDemo.Config.Security;
using MaterialDemo.Config.Security.Messages;
using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Security;
using MaterialDemo.Utils;
using MaterialDemo.Views.Pages.Upms;
using System.Collections.ObjectModel;
using Wpf.Ui;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Base
{
    public partial class HomeViewModel : ObservableRecipient, IRecipient<RefreshUserMessage>
    {

        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new();
        [ObservableProperty]
        private ObservableCollection<object> _footerMenuItems = new();

        [ObservableProperty]
        private string? _userAvaster;

        [ObservableProperty]
        private string? _userName;

        public HomeViewModel(){
            this.IsActive = true;
            this.loadIndexData();
        }


        [RelayCommand]
        private void ToPersonView() => App.GetRequiredService<INavigationService>().Navigate(typeof(PersonViewPage));

        [RelayCommand]
        private void LogOut() => WeakReferenceMessenger.Default.Send(new LogoutMessage());

        public void Receive(RefreshUserMessage message)
        {
            this.loadIndexData();
        }



        private void loadIndexData() {
            SecurityUser? user = SecurityContext.Singleton.getUserInfo();
            if (user != null)
            {
                this.UserAvaster = BaseFileUtil.GetOriFilePath(user.Avatar);
                this.UserName = user.Name;
                if (user.menus.Any())
                {
                    Dictionary<long, NavigationViewItem> navigationMaps = new();

                    List<NavigationViewItem> topItem = new();
                    List<NavigationViewItem> footerItem = new();

                    user.menus.OrderBy(menu => menu.Seq).ToList().ForEach(menu =>
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
                        if (parentItem != null && currentItem != null) parentItem.MenuItems.Add(currentItem);
                    });

                    topItem.ForEach(menu => MenuItems.Add(menu));
                    footerItem.ForEach(menu => FooterMenuItems.Add(menu));
                }
            }
        }
    }
}
