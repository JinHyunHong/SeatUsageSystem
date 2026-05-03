using SeatUsageSystem.Models.Entities;
using System.Linq.Expressions;

namespace SeatUsageSystem.Interfaces
{
    public interface IInOutHistoryRepository
    {
        Task AddAsync(InOutHistory entity);

        Task<int> GetNextSeqAsync(string ymd);

        Task<InOutHistory?> GetLastAsync(int usageId);
    }
}