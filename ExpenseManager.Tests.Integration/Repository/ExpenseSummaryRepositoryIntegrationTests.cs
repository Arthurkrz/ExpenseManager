using Bogus;
using ExpenseManager.Core.Entities;
using ExpenseManager.Infrastructure;
using ExpenseManager.Infrastructure.Repositories;
using ExpenseManager.Service.PredicateBuilder;
using ExpenseManager.Tests.Integration.Utilities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManager.Tests.Integration.Repository
{
    public class ExpenseSummaryRepositoryIntegrationTests : IClassFixture<ExpenseManagerFactory>, IAsyncLifetime
    {
        private readonly Faker _faker = new();
        private readonly ExpenseManagerFactory _factory;

        private IServiceScope _scope = null!;
        private Context _context = null!;
        private ExpenseSummaryRepository _sut = null!;

        public ExpenseSummaryRepositoryIntegrationTests(ExpenseManagerFactory factory)
        {
            _factory = factory;
        }

        [Theory]
        [MemberData(nameof(ExpenseSummaryData.GetSummaries), MemberType = typeof(ExpenseSummaryData))]
        public async Task GetTotalsByCurrencyAsync_ShouldGetTotals(List<Expense> expenses, Dictionary<string, decimal> expectedResult)
        {
            // Arrange
            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, expenses);

            // Act
            var result = await _sut.GetTotalsByCurrencyAsync();

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(ExpenseSummaryData.GetFilteredSummaries), MemberType = typeof(ExpenseSummaryData))]
        public async Task GetTotalsByCurrencyFilterAsync_ShouldGetTotals(List<Expense> expenses, ExpenseFilter filter, Dictionary<string, decimal> expectedResult)
        {
            // Arrange
            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, expenses);

            var filterExpression = ExpensePredicateBuilder.Build(filter);

            // Act
            var result = await _sut.GetTotalsByCurrencyFilterAsync(filterExpression);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        public async Task InitializeAsync()
        {
            await _factory.CleanupAsync();

            _scope = _factory.Services.CreateScope();

            _context = _scope.ServiceProvider
                .GetRequiredService<Context>();

            _sut = new ExpenseSummaryRepository(_context);
        }

        public Task DisposeAsync() =>
            Task.CompletedTask;
    }
}
