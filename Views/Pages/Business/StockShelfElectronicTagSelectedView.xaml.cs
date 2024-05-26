using MaterialDemo.Domain.Models.Entity.Upms;
using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockShelfElectronicTagSelectedView : UserControl
    {
        public ElectronicTagViewModel ViewModel { get; }

        public StockShelfElectronicTagSelectedView(ElectronicTagViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.ViewModel.OnNavigatedTo();
            DataContext = this;
            InitializeComponent();
        }

        private void Submit_Button_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = DataGrid.SelectedItem as ElectronicTag;
            if (selectedRow != null )
            {

            }
            Console.WriteLine();
        }
    }
}
