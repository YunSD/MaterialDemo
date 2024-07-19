using MaterialDemo.ViewModels.Pages.Business;
using MaterialDemo.ViewModels.Windows;
using System.Windows.Threading;
using Wpf.Ui.Controls;

namespace MaterialDemo.Views.Pages.Business
{
    public partial class Index : INavigableView<IndexViewModel>
    {

        private DispatcherTimer timer;
        public IndexViewModel ViewModel { get; }
        public MainWindowViewModel WindowViewModel { get; }

        public Index(IndexViewModel viewModel, MainWindowViewModel WindowViewModel)
        {
            this.ViewModel = viewModel;
            this.WindowViewModel = WindowViewModel;
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

        private void Timer_Tick(object? sender, EventArgs? e)
        {
            // 更新当前时间文本
            CurrentTime.Text = DateTime.Now.ToString("HH:mm:ss  yyyy年MM月dd日  dddd");
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
