using System.Linq.Expressions;

namespace SeatUsageSystem.Interfaces
{
    // 공통 CRUD 정의
    public interface IDatabase<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T?> GetByIdAsync(params object[] keys);
        Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(params object[] keys);
    }
}