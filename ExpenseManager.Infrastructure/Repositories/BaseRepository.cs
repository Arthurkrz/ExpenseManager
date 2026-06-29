using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : Entity
    {
        private readonly Context _context;

        public BaseRepository(Context context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var existingEntity = _context.Set<T>().Find(entity.Id);
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);

            await _context.SaveChangesAsync();
        }

        public async Task<IQueryable<T>> GetAllAsync() =>
            await Task.FromResult(_context.Set<T>().AsQueryable());

    }
}
