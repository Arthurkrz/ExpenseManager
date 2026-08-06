using ExpenseManager.Service.Utilities;
using Moq;
using NuGet.ContentModel;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Tests.Services.Cache
{
    public class RedisCacheServiceTests
    {
        private readonly Mock<IConnectionMultiplexer> _connectionMultiplexerMock;
        private readonly Mock<IDatabase> _databaseMock;
        private readonly RedisCacheService _sut;

        public RedisCacheServiceTests()
        {
            _databaseMock = new Mock<IDatabase>();

            _connectionMultiplexerMock = new Mock<IConnectionMultiplexer>();

            _connectionMultiplexerMock.Setup(x => x.GetDatabase(
                It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_databaseMock.Object);

            _sut = new RedisCacheService(_connectionMultiplexerMock.Object);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldReturnCachedValue_WhenValueExists()
        {
            // Arrange
            const string KEY = "key";

            var expectedValue = new CachedExpense
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Value = 100.0m
            };

            var serializedValue =JsonSerializer.Serialize(expectedValue);

            _databaseMock.Setup(db => db.StringGetAsync(KEY, 
                It.IsAny<CommandFlags>()))
                .ReturnsAsync(serializedValue);

            var isFactoryCalled = false;

            // Act
            var result = await _sut.GetOrCreateAsync(KEY, () =>
            { isFactoryCalled = true; return 
                Task.FromResult(new CachedExpense()); });

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedValue.Id, result.Id);
            Assert.Equal(expectedValue.Name, result.Name);
            Assert.Equal(expectedValue.Value, result.Value);
            Assert.False(isFactoryCalled);

            _databaseMock.Verify(db => db.StringSetAsync(
                It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(), It.IsAny<When>(),
                It.IsAny<CommandFlags>()), Times.Never);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldCreateAndCacheValue_WhenValueNotExists()
        {
            // Arrange
            const string KEY = "key";

            var expectedValue = new CachedExpense
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Value = 100.0m
            };

            Expiration? capturedExpiration = null;

            _databaseMock.Setup(db => db.StringGetAsync(KEY,
                It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            _databaseMock.Setup(db => db.StringSetAsync(KEY,
                It.IsAny<RedisValue>(), It.IsAny<Expiration>(),
                It.IsAny<ValueCondition>(), It.IsAny<CommandFlags>()))
                .Callback<RedisKey, RedisValue, Expiration, ValueCondition, CommandFlags>(
                    (key, value, expiration, valueCondition, commandFlags) => 
                    { capturedExpiration = expiration; })
                .ReturnsAsync(true);

            // Act
            var result = await _sut.GetOrCreateAsync(KEY, () =>
                Task.FromResult(expectedValue));

            // Assert
            Assert.Same(expectedValue, result);

            _databaseMock.Verify(db => db.StringSetAsync(KEY,
                It.Is<RedisValue>(rv => IsSerializedExpense(rv, expectedValue)),
                It.IsAny<Expiration>(), It.IsAny<ValueCondition>(), 
                CommandFlags.None), Times.Once);

            Assert.Equal(TimeSpan.FromHours(24), capturedExpiration.Value);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldUseCustomExpiration()
        {
            // Arrange
            const string KEY = "key";
            var serializedValue = JsonSerializer.Serialize("value");

            var expiration = TimeSpan.FromMinutes(30);
            Expiration? capturedExpiration = null;

            _databaseMock.Setup(db => db.StringGetAsync(KEY, 
                It.IsAny<CommandFlags>()))
                .ReturnsAsync(RedisValue.Null);

            _databaseMock.Setup(db => db.StringSetAsync(KEY,
                It.IsAny<RedisValue>(), It.IsAny<Expiration>(),
                It.IsAny<ValueCondition>(), It.IsAny<CommandFlags>()))
                .Callback<RedisKey, RedisValue, Expiration, ValueCondition, CommandFlags>(
                    (key, value, exp, valueCond, flags) => capturedExpiration = exp)
                .ReturnsAsync(true);

            // Act
            await _sut.GetOrCreateAsync(KEY, 
                () => Task.FromResult("value"), expiration);

            // Assert
            Assert.NotNull(capturedExpiration);

            _databaseMock.Verify(db => db.StringSetAsync(KEY, 
                serializedValue, It.IsAny<Expiration>(), 
                It.IsAny<ValueCondition>(), It.IsAny<CommandFlags>()), 
                Times.Once);

            Assert.Equal(TimeSpan.FromMinutes(30), capturedExpiration.Value);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldReturnDefault_WhenInvalidKey()
        {
            // Arrange 
            var isFactoryCalled = false;

            // Act
            var result = await _sut.GetOrCreateAsync(string.Empty, () =>
            { isFactoryCalled = true; return Task.FromResult(new CachedExpense()); });

            // Assert
            Assert.Null(result);
            Assert.False(isFactoryCalled);

            _databaseMock.Verify(db => db.StringGetAsync(
                It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), 
                    Times.Never);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldReturnDefault_WhenFactoryReturnsNull()
        {
            // Arrange
            const string KEY = "key";
            var factoryCalls = 0;

            // Act
            var firstResult = await _sut.GetOrCreateAsync(KEY, () =>
            { factoryCalls++; return Task.FromResult<string>(null); });

            var secondResult = await _sut.GetOrCreateAsync(KEY, () =>
            { factoryCalls++; return Task.FromResult<string>(null); });

            // Assert
            Assert.Null(firstResult);
            Assert.Null(secondResult);
            Assert.Equal(2, factoryCalls);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldCreateValue_WhenCachedJsonDeserializesToNull()
        {
            // Arrange
            const string KEY = "key";

            Expiration? capturedExpiration = null;

            _databaseMock.Setup(db => db.StringGetAsync(
                It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync((RedisValue)"null");

            _databaseMock.Setup(db => db.StringSetAsync(KEY, 
                It.IsAny<RedisValue>(), It.IsAny<Expiration>(), 
                It.IsAny<ValueCondition>(), It.IsAny<CommandFlags>()))
                .Callback<RedisKey, RedisValue, Expiration, ValueCondition, CommandFlags>(
                    (key, value, exp, valueCond, flags) => capturedExpiration = exp)
                .ReturnsAsync(true);

            // Act
            var result = await _sut.GetOrCreateAsync(KEY, 
                () => Task.FromResult("value"));

            // Assert
            Assert.Equal("value", result);

            _databaseMock.Verify(db => db.StringSetAsync(KEY,
                It.IsAny<RedisValue>(), It.IsAny<Expiration>(), 
                It.IsAny<ValueCondition>(), It.IsAny<CommandFlags>()), Times.Once);

            Assert.Equal(TimeSpan.FromHours(24), capturedExpiration.Value);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldPropagateFactoryException()
        {
            // Arrange
            const string KEY = "key";

            _databaseMock.Setup(db => db.StringGetAsync(KEY, 
                It.IsAny<CommandFlags>())).ReturnsAsync(RedisValue.Null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _sut.GetOrCreateAsync<string>(KEY, () => 
                throw new InvalidOperationException("Could not create value.")));

            Assert.Equal("Could not create value.", exception.Message);

            _databaseMock.Verify(db => db.StringSetAsync(
                It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                It.IsAny<TimeSpan?>(), It.IsAny<When>(),
                It.IsAny<CommandFlags>()), Times.Never);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDeleteRedisKey()
        {
            // Arrange
            const string KEY = "key";

            _databaseMock.Setup(db => db.KeyDeleteAsync(
                It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .ReturnsAsync(true);

            // Act
            await _sut.RemoveAsync(KEY);

            // Assert
            _databaseMock.Verify(x => x.KeyDeleteAsync(
                KEY, It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task RemoveAsync_ShouldNotAccessRedis_WhenInvalidKey()
        {
            // Act
            await _sut.RemoveAsync(string.Empty);
            await _sut.RemoveAsync(null);
            await _sut.RemoveAsync(" ");

            // Assert
            _databaseMock.Verify(x => x.KeyDeleteAsync(
                It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Never);
        }

        private static bool IsSerializedExpense(RedisValue redisValue, CachedExpense expected)
        {
            var deserializedValue =
                JsonSerializer.Deserialize<CachedExpense>(redisValue!);

            return deserializedValue is not null
                && deserializedValue.Id == expected.Id
                && deserializedValue.Name == expected.Name
                && deserializedValue.Value == expected.Value;
        }

    }
}
