using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.DTOs;
using ExpenseManager.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Tests
{
    public class ExchangeServiceTest
    {
        private readonly Mock<IExchangeRateProvider> _exchangeHandlerMock;
        private readonly Mock<ICacheService> _memoryCacheServiceMock;
        private readonly ExchangeService _sut;

        public ExchangeServiceTest()
        {
            _exchangeHandlerMock = new Mock<IExchangeRateProvider>();
            _memoryCacheServiceMock = new Mock<ICacheService>();

            _sut = new ExchangeService(_exchangeHandlerMock.Object, _memoryCacheServiceMock.Object);
        }

        [Fact]
        public async Task ExchangeService_MustReturnExchangeRate_WhenCacheExists()
        {
            // Arrange
            var exchangeResult = new ExchangeResultDTO
            {
                Rates = new Dictionary<string, decimal> { { "BRL", 6.060268m } }
            };

            _exchangeHandlerMock.Setup(x => x.GetExchangeOfDayAsync())
                .ReturnsAsync(exchangeResult);

            //_memoryCacheServiceMock.Setup(x => x.GetOrCreateAsync(
            //    It.IsAny<string>(), It.IsAny<Func<Task<ExchangeResultDTO>>>()))
            //        .ReturnsAsync(exchangeResult);

            // Act & Assert
            Assert.Equal(exchangeResult.Rates, await _sut.GetExchangeAsync());

            //_memoryCacheServiceMock.Verify(x => x.GetOrCreateAsync(
            //    "exchange", It.IsAny<Func<Task<ExchangeResultDTO>>>()), Times.Once);
        }
    }
}
