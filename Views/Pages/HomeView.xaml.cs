using MaterialDemo.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialDemo.Views.Pages
{
    /// <summary>
    /// HomeView.xaml 的交互逻辑
    /// </summary>
    public partial class HomeView : Page
    {

        public List<NavigationItem> NavigationItems { get; set; }

        public HomeView()
        {
            InitializeComponent();
            DataContext = this;

            NavigationItems = new()
            {
                new NavigationItem
                {
                    Title = "Payment",
                    SelectedIcon = PackIconKind.CreditCard,
                    UnselectedIcon = PackIconKind.CreditCardOutline,
                    Notification = 1
                },
                new NavigationItem
                {
                    Title = "Home",
                    SelectedIcon = PackIconKind.Home,
                    UnselectedIcon = PackIconKind.HomeOutline,
                },
                new NavigationItem
                {
                    Title = "Special",
                    SelectedIcon = PackIconKind.Star,
                    UnselectedIcon = PackIconKind.StarOutline,
                },
                new NavigationItem
                {
                    Title = "Shared",
                    SelectedIcon = PackIconKind.Users,
                    UnselectedIcon = PackIconKind.UsersOutline,
                },
                new NavigationItem
                {
                    Title = "Files",
                    SelectedIcon = PackIconKind.Folder,
                    UnselectedIcon = PackIconKind.FolderOutline,
                },
                new NavigationItem
                {
                    Title = "Library",
                    SelectedIcon = PackIconKind.Bookshelf,
                    UnselectedIcon = PackIconKind.Bookshelf,
                },
            };
        }
    }
}
