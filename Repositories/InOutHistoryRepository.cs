using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using SeatUsageSystem.Models.Entities;
using System.Linq.Expressions;

namespace SeatUsageSystem.Repositories
{
    public class InOutHistoryRepository : IInOutHistoryRepository
    {
        private readonly IDatabase<InOutHistory> _database;

        public InOutHistoryRepository(IDatabase<InOutHistory> database)
        {
            _database = database;
        }

        public async Task AddAsync(InOutHistory entity)
        {
            await _database.AddAsync(entity);
        }

        public async Task<int> GetNextSeqAsync(string ymd)
        {
            var list = await _database.FindAsync(x => x.InOutYmd == ymd);

            if (!list.Any())
                return 1;

            return list.Max(x => x.InOutSeq) + 1;
        }

        public async Task<InOutHistory?> GetLastAsync(int usageId)
        {
            var list = await _database.FindAsync(x => x.UsageId == usageId);

            return list
                .OrderByDescending(x => x.InOutYmd)
                .ThenByDescending(x => x.InOutSeq)
                .FirstOrDefault();
        }
    }
}