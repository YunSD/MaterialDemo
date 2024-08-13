using MaterialDemo.ViewModels.Pages.Business;
using Gma.System.MouseKeyHook;

namespace MaterialDemo.Views.Pages.Business
{
    /// <summary>
    /// StockIndex.xaml 的交互逻辑
    /// </summary>
    public partial class StockIndexItemView
    {

        public StockIndexItemViewModel ViewModel { get; }

        private static IKeyboardMouseEvents GlobalHook = Hook.GlobalEvents();
        public StockIndexItemView(StockIndexItemViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();
            this.RegisterMouseHook();
        }

        private void RegisterMouseHook() {
            GlobalHook.MouseMove += GlobalHook_MouseMove;
        }

        private void GlobalHook_MouseMove(object? sender, System.Windows.Forms.MouseEventArgs e)
        {
            GlobalHook.MouseMove -= GlobalHook_MouseMove;
            ViewModel.CloseViewCommand.Execute(null);
        }
    }
}
