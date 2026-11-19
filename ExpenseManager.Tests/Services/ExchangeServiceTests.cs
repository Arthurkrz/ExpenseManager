using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.DTOs;
using ExpenseManager.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Tests.Services
{
    public class ExchangeServiceTests
    {
        private readonly Mock<IExchangeRateProvider> _exchangeProviderMock = new();
        private readonly Mock<ICacheService> _cacheServiceMock = new();
        private readonly IExchangeService _sut;

        public ExchangeServiceTests()
        {
            _sut = new ExchangeService(_exchangeProviderMock.Object, _cacheServiceMock.Object);
        }

        [Fact]
        public async Task GetExchangeAsync_ShouldCallCacheServiceAndRateProviderMethods()
        {
            // Arrange
            var exchangeResult = new ExchangeResultDTO
            {
                Rates = new Dictionary<string, decimal>
                {
                    { "USD", 1.0m },
                    { "EUR", 0.85m }
                }
            };

            _cacheServiceMock.Setup(x => x.GetOrCreateAsync(
                It.IsAny<string>(), It.IsAny<Func<Task<ExchangeResultDTO>>>(), null))
                .ReturnsAsync(exchangeResult);

            // Act
            var result = await _sut.GetExchangeAsync();

            // Assert
            _cacheServiceMock.Verify(x => x.GetOrCreateAsync(
                It.IsAny<string>(), It.IsAny<Func<Task<
                    ExchangeResultDTO>>>(), null), Times.Once);

            Assert.Equal(exchangeResult.Rates, result);
        }

        [Fact]
        public async Task GetExchangeAsync_ShouldReturnEmptyDictionary_WhenReturnedNullRates()
        {
            // Arrange
            var exchangeResult = new ExchangeResultDTO
                { Rates = null };

            _cacheServiceMock.Setup(x => x.GetOrCreateAsync(
                It.IsAny<string>(), It.IsAny<Func<Task<ExchangeResultDTO>>>(), null))
                .ReturnsAsync(exchangeResult);

            // Act
            var result = await _sut.GetExchangeAsync();

            // Assert
            _cacheServiceMock.Verify(x => x.GetOrCreateAsync(
                It.IsAny<string>(), It.IsAny<Func<Task<
                    ExchangeResultDTO>>>(), null), Times.Once);

            Assert.Equal(new Dictionary<string, decimal>(), result);
        }
    }
}
