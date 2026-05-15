using SeatUsageSystem.Models.DTOs;
using SeatUsageSystem.Models.Entities;
using System.Threading.Tasks;

namespace SeatUsageSystem.Interfaces
{
    public interface IUsageRepository
    {
        Task<Usage?> GetActiveUsageAsync(int memberId);

        Task<StartUsageResultDto> StartUsageAsync(int memberId, int seatId);

        Task<EndUsageResultDto> EndUsageAsync(int memberId);

        Task<int> GetTodayUsageCountAsync(int? areaId = null);
        
        Task<List<Usage>> GetTodayUsagesAsync(int? areaId = null);

        Task<List<Usage>> GetUsagesByDateRangeAsync(DateOnly startDate, DateOnly endDate, int? areaId = null);
    }
}