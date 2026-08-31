using ExpenseManager.Tests.Integration.Utilities;

namespace ExpenseManager.Tests.Integration
{
    public class ExpenseControllerIntegrationTests : IClassFixture<ExpenseManagerFactory>, IAsyncLifetime
    {
        private readonly ExpenseManagerFactory _factory;
        private readonly HttpClient _client;

        public ExpenseControllerIntegrationTests(ExpenseManagerFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task IndexAsync_ShouldReturnCorrectExpenseTable()
        {
            // Arrange
            
            
            // Act


            // Assert

        }

        [Fact]
        public async Task FilterAsync_ShouldReturnFilteredExpenseTable()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task FilterAsync_ShouldReturnBadRequest_WhenNoParametersGiven()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task FilterAsync_ShouldReturnBadRequest_WhenErrorsFound()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task CreateAsync_ShouldCreateExpenseReturnOkAndMessage()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBadRequest_WhenInvalidModelState()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task CreateAsync_ShouldReturnBadRequest_WhenErrorsFound()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExpenseReturnOkAndMessage()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnBadRequest_WhenEmptyId()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnBadRequest_WhenErrorsFound()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteExpenseReturnMessageAndRedirectToIndex()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnErrorMessageAndRedirectToIndex_WhenErrorsFound()
        {
            // Arrange


            // Act


            // Assert

        }

        public async Task InitializeAsync() =>
            await _factory.CleanupAsync();

        public Task DisposeAsync() =>
            Task.CompletedTask;
    }
}
