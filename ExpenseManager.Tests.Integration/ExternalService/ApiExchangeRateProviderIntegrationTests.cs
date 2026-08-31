using ExpenseManager.ExternalServices;

namespace ExpenseManager.Tests.Integration.ExternalService
{
    public class ApiExchangeRateProviderIntegrationTests
    {
        private readonly ApiExchangeRateProvider _sut;

        [Fact]
        public async Task GetExchangeOfDayAsync_ShouldReturnMockedExchangeRate()
        {
            // Arrange
            var expectedRates = "";

            // Act
            var result = await _sut.GetExchangeOfDayAsync();

            // Assert
            Assert.Equal(DateTime.Now.Date, result.Date);
            Assert.Equal(expectedRates, result.Rates);
        }
    }
}
