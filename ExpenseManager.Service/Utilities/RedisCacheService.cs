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

        private static readonly TimeSpan _defaultExpiration = TimeSpan.FromHours(24);

        public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer.GetDatabase();
        }

        public async Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> createItem, TimeSpan? expiration = null)
        {
            if (string.IsNullOrWhiteSpace(key)) return default;

            var cachedValue = await _redis.StringGetAsync(key);

            if (!cachedValue.IsNullOrEmpty)
            {
                var deserializedValue = JsonSerializer
                    .Deserialize<T>(cachedValue);

                if (deserializedValue is not null) 
                    return deserializedValue;
            }

            var value = await createItem();

            if (value is null) return default;

            var serializedValue = JsonSerializer.Serialize(value);

            await _redis.StringSetAsync(key, serializedValue, 
                expiration ?? _defaultExpiration);

            return value;
        }

        public async Task RemoveAsync(string key)
        {
            if (string.IsNullOrWhiteSpace(key)) return;

            await _redis.KeyDeleteAsync(key);
        }
    }
}
