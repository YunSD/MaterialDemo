using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockShelfEditorView : UserControl
    {
        public StockShelfEditorViewModel ViewModel { get; }
        private ElectronicTagViewModel TagViewModel { get; }
        private StockMaterialViewModel MaterialViewModel { get; }

        public StockShelfEditorView(StockShelfEditorViewModel viewModel, ElectronicTagViewModel TagViewModel, StockMaterialViewModel MaterialViewModel)
        {
            this.ViewModel = viewModel;
            this.TagViewModel = TagViewModel;
            this.MaterialViewModel = MaterialViewModel;
            this.DataContext = this;
            InitializeComponent();
        }

        private void Tag_Button_Click(object sender, RoutedEventArgs e)
        {
            var confirm = new StockShelfElectronicTagSelectedView(TagViewModel, (row) =>
            {
                if (row != null)
                {
                    ViewModel.ElectronicTagId = row.TagId;
                    ViewModel.ElectronicTagInfo = row.Code;
                }
                else
                {
                    ViewModel.ElectronicTagId = null;
                    ViewModel.ElectronicTagInfo = null;
                }
            });
            DialogHost.Show(confirm, BaseConstant.RootDialog);
        }

        private void Material_Button_Click(object sender, RoutedEventArgs e)
        {
            var confirm = new StockShelfMaterialSelectedView(MaterialViewModel, (row) =>
            {
                if (row != null)
                {
                    ViewModel.StockMaterialId = row.MaterialId;
                    ViewModel.StockMaterialInfo = row.Name;
                }
                else
                {
                    ViewModel.ElectronicTagId = null;
                    ViewModel.ElectronicTagInfo = null;
                }
            });
            DialogHost.Show(confirm, BaseConstant.RootDialog);
        }
    }
}
