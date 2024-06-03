using System.Windows.Controls;

namespace MaterialDemo.Controls;

public partial class WaitingDialog : UserControl
{

    public WaitingDialog()
    {
        DataContext = this;
        InitializeComponent();
    }
}
