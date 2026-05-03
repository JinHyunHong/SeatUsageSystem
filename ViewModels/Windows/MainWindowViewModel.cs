using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Services;
using System.Collections.ObjectModel;
using Wpf.Ui.Controls;

namespace SeatUsageSystem.ViewModels.Windows
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string _applicationTitle = "좌석 이용 시스템";

        [ObservableProperty]
        private string loginMenuText = "로그인";


        [ObservableProperty]
        private ObservableCollection<object> _menuItems = new()
        {
            new NavigationViewItem()
            {
                Content = "홈",
                Icon = new SymbolIcon { Symbol = SymbolRegular.Home24 },
                TargetPageType = typeof(Views.Pages.DashboardPage)
            },
            new NavigationViewItem()
            {
                Content = "좌석 이용",
                Icon = new SymbolIcon { Symbol = SymbolRegular.CheckboxChecked24 },
                TargetPageType = typeof(Views.Pages.UsagePage)
            }   ,         
            new NavigationViewItem()
            {
                Content = "이용 내역(관리자)",
                Icon = new SymbolIcon { Symbol = SymbolRegular.TextBulletListLtr24 },
                TargetPageType = typeof(Views.Pages.UsageListPage)
            }
        };

        [ObservableProperty]
        private ObservableCollection<MenuItem> _trayMenuItems = new()
        {
            new MenuItem { Header = "홈", Tag = "tray_home" }
        };

        public MainWindowViewModel(IAuthService authService)
        {
            _authService = authService;

            _authService.AuthChanged += OnAuthChanged;
        }

        private void OnAuthChanged()
        {
            LoginMenuText = _authService.IsLoggedIn
                ? $"{_authService.CurrentUser?.MemberName}님"
                : "로그인"; // 로그인시 명칭 변경(로그인 -> 사용자님)
        }
    }
}
