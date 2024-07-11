using CommunityToolkit.Mvvm.Messaging;
using MaterialDemo.Config.Security.Messages;
using MaterialDemo.Views.Pages.Base;
using MaterialDemo.Views.Pages.Business;
using MaterialDemo.Views.Pages.Upms;
using Wpf.Ui;

namespace MaterialDemo.ViewModels.Pages.Business
{
    public partial class IndexViewModel : ObservableObject
    {

        [RelayCommand]
        private void ToPageView(string PageType)  {
            Type? type = default;
            if("1".Equals(PageType)) type = typeof(StockIndex);
            if("2".Equals(PageType)) type = typeof(StockShelfViewPage);
            if("3".Equals(PageType)) type = typeof(StockMaterialStatementViewPage);
            if("4".Equals(PageType)) type = typeof(FolderViewPage);
            if(type != null) App.GetRequiredService<INavigationService>().Navigate(type);
        }

        [RelayCommand]
        private void LogOut() => WeakReferenceMessenger.Default.Send(new LogoutMessage());
    }
}
