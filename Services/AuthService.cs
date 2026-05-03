using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.Entities;
using System.Linq.Expressions;

namespace SeatUsageSystem.Services
{
    /// <summary>
    /// 로그인 상태를 관리하는 인증 서비스
    /// </summary>
    public class AuthService : IAuthService
    {
        /// <summary>
        /// 로그인, 로그아웃 상태 변경 델리게이트
        /// </summary>
        public event Action? AuthChanged;

        public Member? CurrentUser { get; private set; }

        /// <summary>
        /// 현재 로그인된 사용자를 반환
        /// 이 프로퍼티는 로그인 상태를 전제로 하며,
        /// 미로그인 상태일 경우 예외를 발생시킨다.
        /// </summary>
        public Member CurrentUserRequired => CurrentUser ?? throw new InvalidOperationException("로그인이 필요합니다.");

        public bool IsLoggedIn => CurrentUser != null;

        public void Login(Member member)
        {
            CurrentUser = member;
            AuthChanged?.Invoke();
        }

        public void Logout()
        {
            CurrentUser = null;
            AuthChanged?.Invoke();
        }
    }
}