using MaterialDemo.Controls;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace MaterialDemo.ViewModels.Pages.Base
{
    public partial class FolderViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<object> navigationCards = new(){
            
        };


        public void LoadNavigationItems(Type? type) {
            NavigationCards.Add(
                new NavigationCard() {
                    Name = "首页",
                    Icon = SymbolRegular.Home24,
                    PageType = typeof(Views.Pages.Base.LoginViewPage)
                }
            );

            NavigationCards.Add(
                new NavigationCard()
                {
                    Name = "首页2",
                    Icon = SymbolRegular.BoxMultipleSearch24,
                    PageType = typeof(Views.Pages.Base.LoginViewPage),
                }
            );
        }
    }
}
