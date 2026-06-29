using System;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Services
{
    public interface IMemoryCacheService
    {
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createItem);
    }
}
