using ExpenseManager.Core.Entities;
using System;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Repositories
{
    public interface IBaseRepository<T> where T : Entity
    {
        Task AddAsync(T entity);

        Task DeleteAsync(T entity);

        Task UpdateAsync(T entity);

        Task<T> GetByIdAsync(Guid id);
    }
}