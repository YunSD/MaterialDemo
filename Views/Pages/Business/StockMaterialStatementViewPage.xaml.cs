using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockMaterialStatementViewPage : INavigableView<StockMaterialStatementViewModel>
    {
        public StockMaterialStatementViewModel ViewModel { get; }

        public StockMaterialStatementViewPage(StockMaterialStatementViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
        }

        private void RecordWayComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            ComboBoxItem? selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;
            var selectedTag = selectedItem.Tag as string;
            if (selectedTag != null )
            {
                ViewModel.SearchWay = (MaterialStatementWayEnum)Enum.Parse(typeof(MaterialStatementWayEnum), selectedTag);
            }
        }
    }
}
