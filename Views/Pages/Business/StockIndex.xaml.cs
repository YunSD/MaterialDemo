using MaterialDemo.ViewModels;
using MaterialDemo.ViewModels.Pages.Business;
using System.Windows.Controls;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    /// <summary>
    /// StockIndex.xaml 的交互逻辑
    /// </summary>
    public partial class StockIndex : INavigableView<StockIndexViewModel>
    {

        private DispatcherTimer timer;
        public StockIndexViewModel ViewModel { get; }
        public DataAcquisitionService DataAcquisition { get; }

        public StockIndex(StockIndexViewModel viewModel, DataAcquisitionService dataAcquisitionService)
        {
            this.ViewModel = viewModel;
            this.DataAcquisition = dataAcquisitionService;

            DataContext = this;
            InitializeComponent();
            this.Unloaded += Page_Unloaded;
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
            if (sender is Border)
            {
                this.ViewModel.ItemViewCommand.Execute(((Border)sender).DataContext);
            }

        }

        private void Timer_Tick(object? sender, EventArgs? e)
        {
            // 更新当前时间文本
            CurrentTime.Text = DateTime.Now.ToString("HH:mm:ss  yyyy年MM月dd日  dddd");
            // 更新控制器状态
            this.ViewModel.ScaleConnectStatus = DataAcquisition.RequestScaleConnectStatus();
            this.ViewModel.ETagConnectStatus = DataAcquisition.RequestEtagConnectStatus();
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            // 停止定时器
            if (timer != null)
            {
                timer.Stop();
            }
        }
    }
}
