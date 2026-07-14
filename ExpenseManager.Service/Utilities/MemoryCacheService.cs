using ExpenseManager.Core.Contracts.Services;
using System;
using System.Threading.Tasks;

namespace ExpenseManager.Service.Utilities
{
    public class MemoryCacheService : ICacheService
    {
        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createItem)
        {
            throw new NotImplementedException();
        }
    }
}
