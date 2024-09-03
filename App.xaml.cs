using log4net;
using MaterialDemo.Config;
using MaterialDemo.Config.Db;
using MaterialDemo.Config.EFDB;
using MaterialDemo.Config.Extensions;
using MaterialDemo.Security;
using MaterialDemo.Services;
using MaterialDemo.ViewModels;
using MaterialDemo.ViewModels.Windows;
using MaterialDemo.Views.Windows;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Threading;
using UiDesktopApp1.Services;
using Wpf.Ui;


namespace MaterialDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddProvider(new Log4NetProvider()));
        private ILog logger = LogManager.GetLogger(nameof(App));

        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => c.SetBasePath(AppContext.BaseDirectory))
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // logging
                services.AddLogging((builder) =>
                {
                    builder.AddConsole();
                    builder.AddLog4Net();
                });

                // Db
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYSQL_ADDR"].ConnectionString;
                services.AddDbContextPool<BaseDbContext>((options) => options
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions => { mySqlOptions.CommandTimeout(1); })
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .AddInterceptors(new BaseMetaDateInterceptor())
                    .LogTo(Console.WriteLine, LogLevel.Debug));
                services.AddUnitOfWork<BaseDbContext>();

                // Main window with navigation
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton(SecurityContext.Singleton);

                services.AddTransientFromNamespace("MaterialDemo.ViewModels", Assembly.GetExecutingAssembly());
                services.AddTransientFromNamespace("MaterialDemo.Views", Assembly.GetExecutingAssembly());
            }).Build();


        public static T? GetService<T>() where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        public static T GetRequiredService<T>() where T : class
        {
            return _host.Services.GetRequiredService<T>();
        }

        public static IServiceScope CreateServiceScope()
        {
            return _host.Services.CreateScope();
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            var splashScreen = new SplashScreen("Assets/cover.png");
            try
            {
                splashScreen.Show(false);
                _host.Start();
            }
            catch (Exception ex)
            {
                StringBuilder builder = new StringBuilder();

                while (true)
                {
                    logger.Error(ex.Message);
                    builder.AppendLine(ex.GetType() + ":" + ex.Message);
                    if (ex.InnerException != null) ex = ex.InnerException;
                    if (ex.InnerException == null) break;
                    ex = ex.InnerException;
                }
                MessageBox.Show("程序初始化失败，即将退出。" + "\n详细信息:\n" + builder.ToString());
                Environment.Exit(0);
                Application.Current.Shutdown();
            }
            finally
            {
                Task.Delay(500).ContinueWith(t =>
                {
                    splashScreen.Close(TimeSpan.Zero);
                });
            }
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();
            _host.Dispose();
        }




        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            logger.Error("程序发送错误：", e.Exception);
        }
    }

}
