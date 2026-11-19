using ExpenseManager.Core.Contracts.Services;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace ExpenseManager.Service.Utilities
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;

        private readonly TimeSpan _defaultExpiration = TimeSpan.FromHours(24);

        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createItem, TimeSpan? expiration = null)
        {
            if (string.IsNullOrWhiteSpace(key)) return default;

            if (_memoryCache.TryGetValue(key, out T cachedValue))
                return cachedValue;

            var value = await createItem();

            if (value is null) return default;

            _memoryCache.Set(key, value, new MemoryCacheEntryOptions
                { AbsoluteExpirationRelativeToNow = expiration ?? _defaultExpiration });

            return value;
        }

        public Task RemoveAsync(string key)
        {
            if (!string.IsNullOrWhiteSpace(key))
                _memoryCache.Remove(key);

            return Task.CompletedTask;
        }
    }
}
