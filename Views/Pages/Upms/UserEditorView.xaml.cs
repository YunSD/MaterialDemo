using MaterialDemo.ViewModels.Pages.Upms;
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

namespace MaterialDemo.Views.Pages.Upms
{
    /// <summary>
    /// UserEditor.xaml 的交互逻辑
    /// </summary>
    public partial class UserEditorView : UserControl
    {
        public UserEditorViewModel ViewModel { get; }

        public UserEditorView(UserEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();
        }

        
    }
}
