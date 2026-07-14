using System;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Services
{
    public interface ICacheService
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createItem);
    }
}
