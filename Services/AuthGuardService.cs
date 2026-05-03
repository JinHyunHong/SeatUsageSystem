using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.Entities;
using SeatUsageSystem.Views.Pages;
using System.Linq.Expressions;
using Wpf.Ui;

namespace SeatUsageSystem.Services
{
    /// <summary>
    /// 인증 상태를 확인하고, 미로그인 상태일 경우 로그인 페이지로 이동시키는 역할을 하는 가드(보호) 서비스
    /// </summary>
    public class AuthGuardService : IAuthGuardService
    {
        private readonly IAuthService _authService;
        private readonly IDialogService _dialogService;
        private readonly INavigationService _navigationService;

        public AuthGuardService(IAuthService authService, IDialogService dialogService, INavigationService navigationService)
        {
            _authService = authService;
            _dialogService = dialogService;
            _navigationService = navigationService;
        }

        /// <summary>
        /// 로그인 여부를 확인하고, 미로그인 시 로그인 페이지로 이동
        /// </summary>
        /// <returns>로그인 상태이면 true, 아니면 false</returns>
        public async Task<bool> CheckAndRedirectLogin()
        {
            if (_authService.IsLoggedIn)
            {
                return true;
            }

            _dialogService.ShowMessage("로그인이 필요합니다.");

            // WPF Navigation lifecycle 충돌 방지를 위해 UI 스레드에서 실행
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                _navigationService.Navigate(typeof(LoginPage));
            });

            return false;
        }
    }
}