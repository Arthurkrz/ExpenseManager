using ExpenseManager.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<IQueryable<T>> GetAllAsync();

        Task AddAsync(T entity);

        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);
    }
}