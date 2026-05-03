using SeatUsageSystem.Models.Entities;

namespace SeatUsageSystem.Interfaces
{
    /// <summary>
    /// 인증 상태를 확인하고, 미로그인 시 로그인 페이지로 유도하는 가드 역할
    /// </summary>
    public interface IAuthGuardService
    {
        /// <summary>
        /// 로그인 상태를 확인하고 필요 시 로그인 페이지로 이동
        /// </summary>
        /// <returns>로그인 상태이면 true, 아니면 false</returns>
        Task<bool> CheckAndRedirectLogin();
    }
}