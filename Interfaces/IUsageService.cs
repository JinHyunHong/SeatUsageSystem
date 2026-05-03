using SeatUsageSystem.Models.DTOs;

namespace SeatUsageSystem.Interfaces
{
    public interface IUsageService
    {
        Task<StartUsageResultDto> StartUsageAsync(int memberId, int seatId);
        Task<EndUsageResultDto> EndUsageAsync(int seatId);
        Task<List<UsageListDto>> GetUsagesByDateRangeAsync(DateOnly startDate, DateOnly endDate, int? areaId = null);
    }
}