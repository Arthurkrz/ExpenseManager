using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using ExpenseManager.Infrastructure;
using ExpenseManager.Infrastructure.Repositories;
using ExpenseManager.Tests.Integration.Builders;
using ExpenseManager.Tests.Integration.Utilities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManager.Tests.Integration.Repository
{
    public class BaseRepositoryIntegrationTests : IClassFixture<ExpenseManagerFactory>, IAsyncLifetime
    {
        private readonly ExpenseManagerFactory _factory;

        private IServiceScope _scope = null!;
        private Context _context = null!;
        private BaseRepository<Expense> _sut = null!;

        public BaseRepositoryIntegrationTests(ExpenseManagerFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task AddAsync_ShouldAddExpense()
        {
            // Arrange
            var expense = new Expense
            {
                Name = "Name",
                Currency = Currency.USD,
                Value = 1,
                Type = ExpenseType.Food,
                ExpenseDate = DateTime.Now,
                Source = "Source"
            };

            // Act
            await _sut.AddAsync(expense);

            // Assert
            var addedExpense = Assert.Single(
                _context.Expenses.ToList());

            addedExpense.Should().BeEquivalentTo(
                expense, options => options
                    .Excluding(x => x.Id));
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteExpense()
        {
            // Arrange
            var expense = ExpenseBuilder.Create()
                .WithAllProperties().Build();

            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, [expense]);

            // Act
            await _sut.DeleteAsync(expense);

            // Assert
            Assert.Empty(_context.Expenses.ToList());
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateExpense()
        {
            // Arrange
            var expense = ExpenseBuilder.Create()
                .WithAllProperties().Build();

            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, [expense]);

            var newExpense = new Expense
            {
                Id = expense.Id,
                Name = "NewName",
                Currency = Currency.CAD,
                Value = expense.Value + 1,
                Type = ExpenseType.Fun,
                ExpenseDate = DateTime.Now.AddDays(-1),
                Source = "NewSource"
            };

            // Act
            await _sut.UpdateAsync(newExpense);

            // Assert
            var updatedExpense = Assert.Single(
                _context.Expenses.ToList());

            updatedExpense.Should().BeEquivalentTo(newExpense);
        }

        [Fact]
        public async Task GetByIdAsync_ShouldGetExpense()
        {
            // Arrange
            var expense = ExpenseBuilder.Create()
                .WithAllProperties().Build();

            await TestDatabaseSeeder.InsertExpensesAsync(
                _factory.Services, [expense]);

            // Act
            var addedExpense = await _sut
                .GetByIdAsync(expense.Id);

            // Assert
            addedExpense.Should().BeEquivalentTo(expense);
        }

        public async Task InitializeAsync()
        {
            await _factory.CleanupAsync();

            _scope = _factory.Services.CreateScope();

            _context = _scope.ServiceProvider
                .GetRequiredService<Context>();

            _sut = new BaseRepository<Expense>(_context);
        }
            
        public Task DisposeAsync() =>
            Task.CompletedTask;
    }
}
