using Bogus;
using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using ExpenseManager.Core.Validators;
using ExpenseManager.Service;
using ExpenseManager.Service.PredicateBuilder;
using ExpenseManager.Tests.ObjectGenerators;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Tests.Services
{
    public class ExpenseServiceTests
    {
        private readonly ExpenseService _sut;
        private readonly Faker _faker = new();
        private readonly ExpenseValidator _validator = new();
        private readonly ExpenseFilterValidator _validatorFilter = new();
        private readonly Mock<IExpenseRepository> _mockRepository = new();

        public ExpenseServiceTests()
        {
            _sut = new ExpenseService(_mockRepository.Object, _validator, _validatorFilter);
        }

        [Fact]
        public async Task CreateExpense_MustAddSuccesfully()
        {
            // Arrange
            var expense = new Expense()
            {
                Name = _faker.Name.FirstName(),
                Currency = _faker.PickRandom<Currency>(),
                Value = _faker.Random.Decimal(1, 1000000),
                Type = _faker.PickRandom<ExpenseType>(),
                ExpenseDate = _faker.Date.Future(),
                Source = _faker.Random.Word(),
            };

            // Act & Assert
            Assert.True((await _sut.CreateExpenseAsync(expense)).Success);
            _mockRepository.Verify(x => x.AddAsync(expense), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExpenseData.GetInvalidExpenses), MemberType = typeof(ExpenseData))]
        public async Task CreateExpense_MustReturnError_WhenInvalidProperty(Expense expense, List<string> errorMessages)
        {
            // Act
            var result = await _sut.CreateExpenseAsync(expense);

            // Assert
            Assert.False(result.Success);

            errorMessages.Should().BeEquivalentTo(result.Errors);
        }

        [Fact]
        public async Task CreateExpense_MustReturnError_WhenNullObject()
        {
            // Arrange
            Expense expense = null;

            // Act
            var result = await _sut.CreateExpenseAsync(expense);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Expense must not be null", result.Errors);
        }

        [Fact]
        public async Task GetExpensesWithFilter_MustGetSuccesfully()
        {
            // Arrange
            var expenseFilter = new ExpenseFilter
            {
                NameContains = "Rice",
                SourceContains = "Walmart",
                ValueRangeStart = 25,
                ValueRangeEnd = 150,
                DateRangeStart = DateTime.Now.AddMonths(-1),
                DateRangeEnd = DateTime.Now,
                Currency = Currency.BRL,
                Type = ExpenseType.Food,
                Month = PurchaseMonth.February
            };

            var filterExpression = ExpensePredicateBuilder.Build(expenseFilter);

            // Act & Assert
            Assert.True((await _sut.GetExpensesWithFilterPagedAsync(expenseFilter)).Success);
            Assert.NotNull(filterExpression);

            _mockRepository.Verify(x => x.GetExpensesWithFilterPagedAsync(
                It.IsAny<Expression<Func<Expense, bool>>>(), 
                It.IsAny<int>(), It.IsAny<int>()), Times.AtLeastOnce);
        }

        [Theory]
        [MemberData(nameof(ExpenseFilterData.GetInvalidFilters), MemberType = typeof(ExpenseFilterData))]
        public async Task GetExpensesWithFilter_MustReturnError_WhenInvalidFilter(ExpenseFilter expenseFilter, List<string> errorMessages)
        {
            // Act
            var result = await _sut.GetExpensesWithFilterPagedAsync(expenseFilter);

            // Assert
            Assert.False(result.Success);

            errorMessages.Should().BeEquivalentTo(result.Errors);
        }

        [Fact]
        public async Task UpdateExpense_MustUpdateSuccesfully()
        {
            // Arrange
            var expense = new Expense
            {
                Id = new Guid(),
                Name = "Name",
                Currency = Currency.EUR,
                Value = 1000,
                Type = ExpenseType.Food,
                ExpenseDate = DateTime.Now,
                Source = "Source"
            };

            _mockRepository.Setup(x => x.GetByIdAsync(expense.Id)).ReturnsAsync(expense);

            // Act & Assert
            Assert.True((await _sut.UpdateExpenseAsync(expense)).Success);
            _mockRepository.Verify(x => x.UpdateAsync(expense), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExpenseData.GetInvalidExpenses), MemberType = typeof(ExpenseData))]
        public async Task UpdateExpense_MustReturnError_WhenInvalidProperty(Expense expense, List<string> errorMessages)
        {
            // Arrange
            _mockRepository.Setup(x => x.GetByIdAsync(expense.Id)).ReturnsAsync(expense);

            // Act
            var result = await _sut.UpdateExpenseAsync(expense);

            // Assert
            Assert.False(result.Success);

            errorMessages.Should().BeEquivalentTo(result.Errors);
        }

        [Fact]
        public async Task UpdateExpense_MustReturnError_WhenExpenseNotFound()
        {
            // Arrange
            var expense = new Expense
            {
                Id = new Guid(),
                Name = "Name",
                Currency = Currency.EUR,
                Value = 1000,
                Type = ExpenseType.Food,
                ExpenseDate = DateTime.Now,
                Source = "Source"
            };

            // Act
            var result = await _sut.UpdateExpenseAsync(expense);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Expense not found.", result.Errors);
        }

        [Fact]
        public async Task DeleteExpense_MustDeleteSuccesfully()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expense = new Expense();

            _mockRepository.Setup(x => x.GetByIdAsync(id)).ReturnsAsync(expense);

            // Act & Assert
            Assert.True((await _sut.DeleteExpenseAsync(id)).Success);
            _mockRepository.Verify(x => x.DeleteAsync(expense), Times.Once);
        }

        [Fact]
        public async Task DeleteExpense_MustReturnError_WhenObjectNotFound()
        {
            // Act
            var result = await _sut.DeleteExpenseAsync(Guid.NewGuid());

            // Assert
            Assert.False(result.Success);
            Assert.Contains("ID does not match any expense.", result.Errors);
        }

        [Fact]
        public async Task List_MustCallListRepositoryMethod()
        {
            // Act 
            await _sut.GetPagedAsync(1, 1);

            // Assert
            _mockRepository.Verify(x => x.GetExpensesPagedAsync(
                It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }
    }
}