using MaterialDemo.Controls;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Base
{
    public partial class FolderViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<object> navigationCards = new(){
            new NavigationCard()
            {
                Name = "首页",
                Icon = SymbolRegular.Home24,
                PageType = typeof(Views.Pages.Base.LoginViewPage)
            },

            new NavigationCard()
            {
                Name = "物料管理",
                Icon = SymbolRegular.BoxMultipleSearch24,
                PageType = typeof(Views.Pages.Business.StockMaterialViewPage),
            },
            new NavigationCard()
            {
                Name = "标签管理",
                Icon = SymbolRegular.TagMultiple24,
                PageType = typeof(Views.Pages.Business.ElectronicTagViewPage),
            },
        };
    }
}
