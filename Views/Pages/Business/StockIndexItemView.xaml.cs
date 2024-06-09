using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    /// <summary>
    /// StockIndex.xaml 的交互逻辑
    /// </summary>
    public partial class StockIndexItemView : INavigableView<StockIndexViewModel>
    {

        private DispatcherTimer timer;
        public StockIndexViewModel ViewModel { get; }

        public StockIndexItemView(StockIndexViewModel viewModel)
        {
            this.ViewModel = viewModel;
            DataContext = this;
            InitializeComponent();

            // 创建一个 DispatcherTimer，间隔为一秒
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            this.Timer_Tick(null, null);
            // 启动定时器
            timer.Start();
        }

        private void Border_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void Timer_Tick(object? sender, EventArgs? e)
        {
            // 更新当前时间文本
            CurrentTime.Text = DateTime.Now.ToString("HH:mm:ss  yyyy年MM月dd日  dddd");
        }
    }
}
