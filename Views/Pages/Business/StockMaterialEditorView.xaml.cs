using MaterialDemo.ViewModels.Pages.Business;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using HandyControl.Controls;
using MaterialDemo.Utils;

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

            BasePageUtil.ShowImageSelector(FormImageSelector, ViewModel.Image);
        }

        private void ImageSelector_ImageSelected(object sender, RoutedEventArgs e)
        {
            if (sender is HandyControl.Controls.ImageSelector) {
                Uri imageUri = ((HandyControl.Controls.ImageSelector)sender).Uri;
                if (imageUri != null)
                {
                    this.ViewModel.Image = imageUri.LocalPath;
                }
            }
        }
    }
}
