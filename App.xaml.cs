using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Repositories;
using SeatUsageSystem.Services;
using SeatUsageSystem.ViewModels.Pages;
using SeatUsageSystem.ViewModels.Windows;
using SeatUsageSystem.Views.Pages;
using SeatUsageSystem.Views.Windows;
using System.IO;
using System.Reflection;
using System.Windows.Threading;
using Wpf.Ui;
using Wpf.Ui.DependencyInjection;

namespace SeatUsageSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(AppContext.BaseDirectory); })
            .ConfigureServices((context, services) =>
            {
                services.AddNavigationViewPageProvider();

                services.AddHostedService<ApplicationHostService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                services.AddDbContext<AppDbContext>();
                services.AddScoped<IUsageService, UsageService>();
                services.AddScoped(typeof(IDatabase<>), typeof(BaseRepository<>));

                // 인증 서비스 전역 공유
                services.AddSingleton<IAuthService, AuthService>();

                // 인증 상태를 확인하고, 미로그인 상태일 경우 로그인 페이지로 이동시키는 역할을 하는 가드(보호) 서비스
                services.AddSingleton<IAuthGuardService, AuthGuardService>();

                // 다이얼 로그 서비스 전역 공유
                services.AddSingleton<IDialogService, DialogService>();

                services.AddScoped<IUsageRepository, UsageRepository>();
                services.AddScoped<IInOutHistoryRepository, InOutHistoryRepository>();
                services.AddScoped<IDoorService, DoorService>();

                // Main window with navigation
                services.AddSingleton<INavigationWindow, MainWindow>();
                services.AddSingleton<MainWindowViewModel>();

                services.AddScoped<DashboardPage>();
                services.AddScoped<DashboardViewModel>();
                
                services.AddScoped<UsagePage>();
                services.AddScoped<UsageViewModel>();
                
                services.AddScoped<UsageListPage>();
                services.AddScoped<UsageListViewModel>();

                services.AddScoped<LoginPage>();
                services.AddScoped<LoginViewModel>();

                services.AddScoped<SettingsPage>();
                services.AddScoped<SettingsViewModel>();
            }).Build();

        /// <summary>
        /// Gets services.
        /// </summary>
        public static IServiceProvider Services
        {
            get { return _host.Services; }
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            try
            {
                await _host.StartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); // ⭐ 이거 꼭
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
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }
    }
}
