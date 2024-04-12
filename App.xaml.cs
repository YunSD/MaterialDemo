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
using System.Configuration;
using MaterialDemo.Config;
using MaterialDemo.Models.Entity;
using MaterialDemo.Config.UnitOfWork;
using MaterialDemo.ViewModels.Pages;
using MahApps.Metro.Controls.Dialogs;
using HandyControl.Controls;
using MessageBox = System.Windows.MessageBox;
using MaterialDemo.Security;
using MaterialDemo.Views.Pages;


namespace MaterialDemo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
       
        private static readonly ILoggerFactory loggerFactory = LoggerFactory.Create(builder => builder.AddProvider(new Log4NetProvider()));

        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)))
            .ConfigureServices((context, services) =>
            {
                services.AddHostedService<ApplicationHostService>();
                // logging
                services.AddLogging((builder)=>{ 
                    builder.AddConsole();
                    builder.AddLog4Net();
                });
                string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["mysql"].ConnectionString;
                // Db
                services.AddDbContextPool<BaseDbContext>((options) => options
                    .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTrackingWithIdentityResolution)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging()
                    .LogTo(Console.WriteLine, LogLevel.Debug));
                services.AddUnitOfWork<BaseDbContext>();
                //services.AddCustomRepository<SysUser,Repository<SysUser>>();


                // Main window with navigation
                services.AddSingleton<MainWindow>();
                services.AddSingleton<LoginView>();
                services.AddSingleton<HomeView>();
                services.AddSingleton(SecurityUser.SECURITY_USER);
                // Model
                services.AddSingleton<MainWindowViewModel>();
                services.AddSingleton<LoginViewModel>();


            }).Build();


        public static T? GetService<T>()
         where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private void OnStartup(object sender, StartupEventArgs e)
        {
            _host.Start();
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
            Console.Write(e.Exception);
            e.Handled = true;
            MessageBox.Show(e.Exception.Message);
            if (e.Exception is MySqlConnector.MySqlException) {
                Application.Current.Shutdown();
            }
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }

}
