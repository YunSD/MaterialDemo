using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using MaterialDemo.Services;
using MaterialDemo.Views.Pages.Login;
using MaterialDemo.Views.Windows;
using MaterialDemo.ViewModels.Windows;
using Microsoft.Extensions.Logging;
using MaterialDemo.Config.EFDB;
using Microsoft.EntityFrameworkCore;
using MaterialDemo.Config;
using MaterialDemo.Security;
using MaterialDemo.Views.Pages.Base;
using MaterialDemo.ViewModels.Pages.Home;
using UiDesktopApp1.Services;
using MaterialDemo.Config.DependencyModel;
using log4net;
using MaterialDemo.ViewModels.Pages.Base;
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
            .ConfigureAppConfiguration(c => c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)))
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();
                services.AddSingleton<IPageService, PageService>();
                services.AddSingleton<INavigationService, NavigationService>();

                // logging
                services.AddLogging((builder)=>{ 
                    builder.AddConsole();
                    builder.AddLog4Net();
                });

                // Db
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
                services.AddDbContextPool<BaseDbContext>((options) => options
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Debug));
                services.AddUnitOfWork<BaseDbContext>();

                // Main window with navigation
                services.AddSingleton<MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddSingleton(SecurityUser.SECURITY_USER);

                services.AddSingleton<LoginView>();
                services.AddSingleton<LoginViewModel>();

                services.AddSingleton<HomeView>();
                services.AddSingleton<HomeViewModel>();

                services.AddTransientFromNamespace("MaterialDemo.ViewModels", Assembly.GetExecutingAssembly());
                services.AddTransientFromNamespace("MaterialDemo.Views", Assembly.GetExecutingAssembly());


            }).Build();


        public static T? GetService<T>() where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                _host.Start();
            }catch (Exception ex)
            {
                logger.Error(ex);
                MessageBox.Show("程序初始化失败，即将退出。" + "\ncause:" + ex.Message);
                Application.Current.Shutdown();
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
            logger.Error(e);
            MessageBox.Show(e.Exception.Message);
        }
    }

}
