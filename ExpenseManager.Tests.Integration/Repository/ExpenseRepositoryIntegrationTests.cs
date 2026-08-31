using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using ExpenseManager.Infrastructure;
using ExpenseManager.Infrastructure.Repositories;
using ExpenseManager.Service.PredicateBuilder;
using ExpenseManager.Tests.Integration.Builders;
using ExpenseManager.Tests.Integration.Utilities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManager.Tests.Integration.Repository
{
    public class ExpenseRepositoryIntegrationTests : IClassFixture<ExpenseManagerFactory>, IAsyncLifetime
    {
        private readonly ExpenseManagerFactory _factory;

        private IServiceScope _scope = null!;
        private Context _context = null!;
        private ExpenseRepository _sut = null!;

        public ExpenseRepositoryIntegrationTests(ExpenseManagerFactory factory) 
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetExpensesPagedAsync_ShouldGetPagedExpenses()
        {
            // Arrange
            var expenses = ExpenseBuilder.Create()
                .WithAllProperties().BuildMany(15);

            var expectedExpenses = expenses.ToList()
                .Where(x => x.Value >= 11 && x.Value <= 15);

            // Act
            var paginatedExpenses = await _sut
                .GetExpensesPagedAsync(2, 5);

            // Assert
            Assert.Equal(2, paginatedExpenses.PageNumber);
            Assert.Equal(5, paginatedExpenses.PageSize);
            Assert.Equal(5, paginatedExpenses.TotalCount);

            paginatedExpenses.Items.Should()
                .BeEquivalentTo(expectedExpenses);
        }

        [Theory]
        [MemberData(nameof(ExpenseFilterData.GetFiltersAll), MemberType = typeof(ExpenseFilterData))]
        public async Task GetExpensesWithFilterPagedAsync_WithNoProperties_ShouldGetFilteredPagedAllExpenses(List<Expense> expenses, ExpenseFilter filter, PaginatedResult<Expense> expectedPaginatedResult)
        {
            // Arrange
            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, expenses);

            var filterExpression = ExpensePredicateBuilder.Build(filter);

            // Act
            var result = await _sut.GetExpensesWithFilterPagedAsync(
                filterExpression, filter.PageNumber, filter.PageSize);

            // Assert
            Assert.Equal(3, result.PageNumber);
            Assert.Equal(5, result.PageSize);

            result.Items.Should().BeEquivalentTo(
                expectedPaginatedResult.Items);
        }

        [Theory]
        [MemberData(nameof(ExpenseFilterData.GetFiltersEach), MemberType = typeof(ExpenseFilterData))]
        public async Task GetExpensesWithFilterPagedAsync_WithOnlyOnePropertyEach_ShouldGetFilteredPagedExpenses(List<Expense> expenses, ExpenseFilter filter, PaginatedResult<Expense> expectedPaginatedResult)
        {
            // Arrange
            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, expenses);

            var filterExpression = ExpensePredicateBuilder.Build(filter);

            // Act
            var result = await _sut.GetExpensesWithFilterPagedAsync(
                filterExpression, filter.PageNumber, filter.PageSize);

            // Assert
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(5, result.PageSize);

            result.Items.Should().BeEquivalentTo(
                expectedPaginatedResult.Items);
        }

        [Theory]
        [MemberData(nameof(ExpenseFilterData.GetFiltersOne), MemberType = typeof(ExpenseFilterData))]
        public async Task GetExpensesWithFilterPagedAsync_WithAllProperties_ShouldGetFilteredPagedOneExpense(List<Expense> expenses, ExpenseFilter filter, PaginatedResult<Expense> expectedPaginatedResult)
        {
            // Arrange
            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, expenses);

            var filterExpression = ExpensePredicateBuilder.Build(filter);

            // Act
            var result = await _sut.GetExpensesWithFilterPagedAsync(
                filterExpression, filter.PageNumber, filter.PageSize);

            // Assert
            Assert.Equal(3, result.PageNumber);
            Assert.Equal(1, result.PageSize);
            Assert.Single(result.Items);

            result.Items.Should().BeEquivalentTo(
                expectedPaginatedResult.Items);
        }

        [Theory]
        [MemberData(nameof(ExpenseFilterData.GetFiltersNone), MemberType = typeof(ExpenseFilterData))]
        public async Task GetExpensesWithFilterPagedAsync_WithProperties_ShouldGetFilteredPagedNoExpenses(List<Expense> expenses, ExpenseFilter filter)
        {
            // Arrange
            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, expenses);

            var filterExpression = ExpensePredicateBuilder.Build(filter);

            // Act
            var result = await _sut.GetExpensesWithFilterPagedAsync(
                filterExpression, filter.PageNumber, filter.PageSize);

            // Assert
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(5, result.PageSize);
            Assert.Empty(result.Items);
        }

        public async Task InitializeAsync()
        {
            await _factory.CleanupAsync();

            _scope = _factory.Services.CreateScope();

            _context = _scope.ServiceProvider
                .GetRequiredService<Context>();

            _sut = new ExpenseRepository(_context);
        }

        public Task DisposeAsync() =>
            Task.CompletedTask;
    }
}
