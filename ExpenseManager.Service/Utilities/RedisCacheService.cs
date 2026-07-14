using ExpenseManager.Core.Contracts.Services;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseManager.Service.Utilities
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _redis;
        private readonly TimeSpan _cacheDuration = TimeSpan.FromHours(24);

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer.GetDatabase();
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createItem)
        {
            var cachedValue = await _redis.StringGetAsync(key);
            if (cachedValue.HasValue)
            {
                return JsonSerializer.Deserialize<T>(cachedValue);
            }

            T value = await createItem();

            var serializedValue = JsonSerializer.Serialize(value);
            await _redis.StringSetAsync(key, serializedValue, _cacheDuration);

            return value;
        }
    }
}
