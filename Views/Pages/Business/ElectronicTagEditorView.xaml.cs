using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class ElectronicTagEditorView : UserControl
    {
        public ElectronicTagEditorViewModel ViewModel { get; }

        public ElectronicTagEditorView(ElectronicTagEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }
    }
}
