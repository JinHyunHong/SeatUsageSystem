using Microsoft.EntityFrameworkCore;
using SeatUsageSystem.Data;
using SeatUsageSystem.Interfaces;
using System.Linq.Expressions;

namespace SeatUsageSystem.Repositories
{
    // 공통 CRUD는 한 번만 구현함
    public class BaseRepository<T> : IDatabase<T> where T : class
    {
        protected readonly AppDbContext _context;

        public BaseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<T?> GetByIdAsync(params object[] keys)
        {
            return await _context.Set<T>().FindAsync(keys);
        }

        // 조건식(Expression)을 기반으로 엔티티를 조회하는 공통 메서드
        // LINQ를 그대로 전달받아 타입 안전성과 유연한 조회를 동시에 제공
        public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>()
                .Where(predicate)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(params object[] keys)
        {
            var entity = await _context.Set<T>().FindAsync(keys);

            if (entity != null)
            {
                _context.Set<T>().Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}