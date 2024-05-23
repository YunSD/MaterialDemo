using MaterialDemo.Utils;
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

            BasePageUtil.ShowImageSelector(MaterialImageSelector, ViewModel.Image);
        }

        private void ImageSelector_ImageSelected(object sender, RoutedEventArgs e)
        {
            if (sender is HandyControl.Controls.ImageSelector)
            {
                Uri imageUri = ((HandyControl.Controls.ImageSelector)sender).Uri;
                if (imageUri != null)
                {
                    this.ViewModel.Image = imageUri.LocalPath;
                }
            }
        }
    }
}
