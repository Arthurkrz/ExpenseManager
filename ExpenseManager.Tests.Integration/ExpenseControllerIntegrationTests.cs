using ExpenseManager.Tests.Integration.Builders;
using ExpenseManager.Tests.Integration.Utilities;
using ExpenseManager.Web.Models;
using ExpenseManager.Web.Models.Enum;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

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
            var expenses = ExpenseBuilder.Create()
                .WithAllProperties().BuildMany(10);

            var expectedRows = expenses.Select(
                ExpenseTableRow.FromExpense).ToList();

            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, expenses);

            // Act
            var response = await _client
                .GetAsync("/Expense/Index");

            // Assert
            response.EnsureSuccessStatusCode();

            var expenseTableRows = await ExpenseTableParser
                .ParseAsync(response);

            expenseTableRows.Should()
                .BeEquivalentTo(expectedRows);

            Assert.Equal(expenseTableRows.Count, 
                expectedRows.Count());
        }

        [Fact]
        public async Task IndexAsync_ShouldReturnCorrectPage_WhenSpecifiedPageNumber()
        {
            // Arrange
            var expenses = ExpenseBuilder.Create()
                .WithAllProperties().BuildMany(25);

            var expectedRows = expenses.Skip(10).Take(10)
                .Select(ExpenseTableRow.FromExpense).ToList();

            // Act
            var response = await _client.GetAsync(
                "/Expense/Index?pageNumber=2&pageSize=10");

            // Assert
            response.EnsureSuccessStatusCode();

            var rows = await ExpenseTableParser
                .ParseAsync(response);

            rows.Should().BeEquivalentTo(expectedRows);
        }

        [Fact]
        public async Task IndexAsync_ShouldReturnNoExpensesMessage_WhenNoExpensesExist()
        {
            // Act
            var response = await _client
                .GetAsync("/Expense/Index");

            // Assert
            response.EnsureSuccessStatusCode();

            var document = await HtmlParser.ParseAsync(response);

            Assert.Null(document.QuerySelector(".expense-table"));

            var message = document.QuerySelector(
                ".no-expense-found")?.TextContent;
            
            Assert.Equal("No expenses found!", message);
        }

        [Fact]
        public async Task IndexAsync_ShouldReturnBadRequest_WhenMissingAntiForgeryToken()
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
        public async Task FilterAsync_ShouldReturnCorrectFilteredExpensesPage()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task FilterAsync_ShouldReturnNoExpensesMessage_WhenNoExpensesFiltered()
        {
            // Arrange
            var filterVM = new ExpenseFilterViewModel
            { NameContains = "NameContains" };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/Expense/Filter", filterVM);

            // Assert
            response.EnsureSuccessStatusCode();

            var document = await HtmlParser.ParseAsync(response);

            Assert.Null(document.QuerySelector(".expense-table"));

            var message = document.QuerySelector(
                ".no-expense-found")?.TextContent;

            Assert.Equal("No expenses found!", message);
        }

        [Fact]
        public async Task FilterAsync_ShouldReturnBadRequest_WhenNoParametersGiven()
        {
            // Act
            var response = await _client.PostAsJsonAsync(
                "/Expense/Filter", new ExpenseFilterViewModel());

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, 
                response.StatusCode);

            var errorObj = await response.Content
                .ReadFromJsonAsync<dynamic>();

            var errors = errorObj!.errors 
                as IEnumerable<string>;

            Assert.Single(errors!);

            Assert.Equal("Add at least 1 search parameter.", errors!.First());
        }

        [Fact]
        public async Task FilterAsync_ShouldReturnBadRequest_WhenErrorsFound()
        {
            // Arrange
            var filterVM = new ExpenseFilterViewModel
            { ValueStringRangeEnd = "1000001" };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/Expense/Filter", filterVM);

            // Assert
            var errorObj = await response.Content
                .ReadFromJsonAsync<dynamic>();

            var errors = errorObj!.errors 
                as IEnumerable<string>;

            Assert.Single(errors!);

            Assert.Equal("End of range for value " +
                "must be lower than a million",
                errors!.First());
        }

        [Fact]
        public async Task FilterAsync_ShouldReturnBadRequest_WhenMissingAntiForgeryToken()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task CreateAsync_ShouldCreateExpenseReturnOkAndMessage()
        {
            // Arrange
            var id = Guid.NewGuid();

            var expenseVM = new ExpenseViewModel
            {
                Id = id,
                Name = "Name",
                Currency = CurrencyVM.USD,
                ValueString = "100.00",
                Type = ExpenseTypeVM.Food,
                Source = "Source",
                ExpenseDate = DateTime.Now
            };

            // Act
            var response = await _client.PostAsJsonAsync(
                "/Expense/Create", expenseVM);

            // Assert
            response.EnsureSuccessStatusCode();
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
        public async Task CreateAsync_ShouldReturnBadRequest_WhenMissingAntiForgeryToken()
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
        public async Task UpdateAsync_ShouldReturnBadRequest_WhenInvalidModelState()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnBadRequest_WhenMissingAntiForgeryToken()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteExpenseReturnOkAndMessage()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnBadRequest_WhenErrorsFound()
        {
            // Arrange


            // Act


            // Assert

        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnBadRequest_WhenMissingAntiForgeryToken()
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
