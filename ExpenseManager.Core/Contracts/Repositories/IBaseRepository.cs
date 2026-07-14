using ExpenseManager.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task<List<T>> GetAllAsync();

        Task AddAsync(T entity);

        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);
    }
}