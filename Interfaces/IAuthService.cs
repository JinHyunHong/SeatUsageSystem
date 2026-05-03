using SeatUsageSystem.Models.Entities;

namespace SeatUsageSystem.Interfaces
{
    /// <summary>
    /// 인증(로그인/로그아웃) 상태 관리 인터페이스
    /// </summary>
    public interface IAuthService
    {
        event Action? AuthChanged;

        Member? CurrentUser { get; }

        /// <summary>
        /// 현재 로그인된 사용자를 반환
        /// 이 프로퍼티는 로그인 상태를 전제로 하며,
        /// 미로그인 상태일 경우 예외를 발생시킨다.
        /// </summary>
        Member CurrentUserRequired { get; }

        bool IsLoggedIn { get; }

        void Login(Member member);

        void Logout();
    }
}