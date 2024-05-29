using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Base
{
    public partial class FolderViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<object> menuItems = new(){
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
        };
    }
}
