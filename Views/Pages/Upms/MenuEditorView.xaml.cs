using MaterialDemo.Domain.Models.Entity;
using MaterialDemo.ViewModels.Pages.Upms;
using MaterialDemo.ViewModels.Pages.Upms.VObject;
using Microsoft.Extensions.Primitives;
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
    public partial class MenuEditorView : UserControl
    {
        public MenuEditorViewModel ViewModel { get; }

        public MenuEditorView(MenuEditorViewModel viewModel)
        {
            this.ViewModel = viewModel;
            this.DataContext = this;
            InitializeComponent();

            // combox index
            if (ViewModel.Parents != null && ViewModel.ParentId != null) {
                int index = ViewModel.Parents.Select(p => p.MenuId).ToList().IndexOf(ViewModel.ParentId);
                if (index > -1) ParentCombo.SelectedIndex = index;  
            }

            PositionCombo.SelectedIndex = ((int)ViewModel.Position);
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            var selectedParent = comboBox.SelectedItem as SysMenuViewInfo;
            if (selectedParent == null) {
                ViewModel.ParentId = 0;
                ViewModel.ParentName = null;
                return;
            }
            ViewModel.ParentId = selectedParent.MenuId;
            ViewModel.ParentName = selectedParent.Name; 
        }

        private void PositionCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox == null) return;
            ComboBoxItem selectedItem = comboBox.SelectedItem as ComboBoxItem;
            if (selectedItem == null) return;
            var selectedParent = selectedItem.Content as string;
            if (selectedParent != null)
            {
                ViewModel.Position = (MenuPositionEnum)Enum.Parse(typeof(MenuPositionEnum), selectedParent);
            }
        }
    }
}
