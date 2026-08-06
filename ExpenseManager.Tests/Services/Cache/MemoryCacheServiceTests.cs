using ExpenseManager.Service.Utilities;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Tests.Services.Cache
{
    public class MemoryCacheServiceTests
    {
        private readonly MemoryCache _memoryCache;
        private readonly MemoryCacheService _sut;

        public MemoryCacheServiceTests()
        {
            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _sut = new MemoryCacheService(_memoryCache);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldCreateAndCacheValue_WhenKeyNotExist()
        {
            // Arrange
            const string KEY = "key";
            var factoryCalls = 0;

            // Act
            var firstResult = await _sut.GetOrCreateAsync(KEY, 
                () => { factoryCalls++; return 
                    Task.FromResult("value"); });


            var secondResult = await _sut.GetOrCreateAsync(KEY,
                () => { factoryCalls++; return 
                    Task.FromResult("new value"); });

            // Assert
            Assert.Equal("value", firstResult);
            Assert.Equal("value", secondResult);
            Assert.Equal(1, factoryCalls);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task GetOrCreateAsync_ShouldReturnDefault_WhenKeyNullEmptyOrWhitespace(string key)
        {
            // Arrange
            var isFactoryCalled = false;

            // Act
            var result = await _sut.GetOrCreateAsync(key, 
                () => { isFactoryCalled = true; 
                        return Task.FromResult("value"); });

            // Assert
            Assert.Null(result);
            Assert.False(isFactoryCalled);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldNotCacheValue_WhenFactoryReturnsNull()
        {
            // Arrange
            const string KEY = "key";
            var factoryCalls = 0;

            // Act
            var firstResult = await _sut.GetOrCreateAsync(KEY,
                () => { factoryCalls++; return 
                    Task.FromResult<string>(null); });

            var secondResult = await _sut.GetOrCreateAsync(KEY, 
                () => { factoryCalls++; return
                    Task.FromResult<string>(null); });

            // Assert
            Assert.Null(firstResult);
            Assert.Null(secondResult);
            Assert.Equal(2, factoryCalls);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldUseCustomExpiration()
        {
            // Arrange
            const string KEY = "key";
            const string VALUE = "value";

            var expiration = TimeSpan.FromMinutes(30);

            object cachedValue = null;

            var memoryCacheMock = new Mock<IMemoryCache>();
            var cacheEntryMock = new Mock<ICacheEntry>();

            cacheEntryMock.SetupProperty(entry => entry.Value)
                .SetupProperty(entry => entry.AbsoluteExpirationRelativeToNow);

            memoryCacheMock.Setup(mc => 
                mc.CreateEntry(It.IsAny<object>()))
                .Returns(cacheEntryMock.Object);

            var sut = new MemoryCacheService(memoryCacheMock.Object);

            memoryCacheMock.Setup(cache => cache.TryGetValue(
                KEY, out cachedValue)).Returns(false);

            // Act
            var result = await sut.GetOrCreateAsync(KEY,
                () => Task.FromResult(VALUE), expiration);

            // Assert
            Assert.Equal(VALUE, result);

            memoryCacheMock.Verify(mc => mc
                .CreateEntry(KEY), Times.Once);

            Assert.Equal(expiration, cacheEntryMock
                .Object.AbsoluteExpirationRelativeToNow);

            Assert.Equal(VALUE, cacheEntryMock.Object.Value);
        }

        [Fact]
        public async Task GetOrCreateAsync_ShouldCacheComplexObject()
        {
            // Arrange
            const string KEY = "key";
            
            var expense = new CachedExpense
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Value = 150.75m
            };

            // Act
            var firstResult = await _sut.GetOrCreateAsync(KEY,
                () => Task.FromResult(expense));

            var secondResult = await _sut.GetOrCreateAsync(KEY,
                () => Task.FromResult(new CachedExpense()));

            // Assert
            Assert.Same(expense, firstResult);
            Assert.Same(expense, secondResult);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveCachedValue()
        {
            // Arrange
            const string KEY = "key";

            await _sut.GetOrCreateAsync(KEY, () => 
                Task.FromResult("value"));

            // Act
            await _sut.RemoveAsync(KEY);

            var result = await _sut.GetOrCreateAsync(KEY, () =>
                Task.FromResult("new value"));

            // Assert
            Assert.Equal("new value", result);
        }

        [Fact]
        public async Task RemoveAsync_ShouldNotThrow_WhenInvalidKey()
        {
            // Act
            var exception = await Record.ExceptionAsync(async () =>
            {
                await _sut.RemoveAsync(null);
                await _sut.RemoveAsync(string.Empty);
                await _sut.RemoveAsync(" ");
            });

            // Assert
            Assert.Null(exception);
        }
    }
}
