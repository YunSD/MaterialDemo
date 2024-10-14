using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.Utils;
using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class StockMaterialEditorView : UserControl
    {
        public StockMaterialEditorViewModel ViewModel { get; }

        public StockMaterialEditorView(StockMaterialEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();

            // 初始化图片
            BasePageUtil.ShowImageSelector(MaterialImageSelector, ViewModel.Image);
        }

        private void ImageSelector_ImageSelected(object sender, RoutedEventArgs e)
        {
            if (sender is HandyControl.Controls.ImageSelector)
            {
                Uri imageUri = ((HandyControl.Controls.ImageSelector)sender).Uri;
                if (imageUri != null)
                {
                    // image copy
                    this.ViewModel.Image = BaseFileUtil.UpdateFile(imageUri.LocalPath);
                }
            }
        }
    }
}
