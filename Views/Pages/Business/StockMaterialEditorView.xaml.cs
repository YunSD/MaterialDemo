using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    /// <summary>
    /// UserEditor.xaml 的交互逻辑
    /// </summary>
    public partial class StockMaterialEditorView : UserControl
    {
        public StockMaterialEditorViewModel ViewModel { get; }

        public StockMaterialEditorView(StockMaterialEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }

        private void ImageSelector_ImageSelected(object sender, RoutedEventArgs e)
        {
            Console.WriteLine();
        }
    }
}
