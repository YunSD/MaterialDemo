using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockShelfEditorView : UserControl
    {
        private StockShelfEditorViewModel ViewModel { get; }
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
            var confirm = new StockShelfElectronicTagSelectedView(TagViewModel);
            DialogHost.Show(confirm, BaseConstant.RootDialog);
        }
    }
}
