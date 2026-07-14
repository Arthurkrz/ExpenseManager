using Bogus;
using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using ExpenseManager.Core.Validators;
using ExpenseManager.Service;
using ExpenseManager.Service.PredicateBuilder;
using ExpenseManager.Tests.ObjectGenerators;
using FluentValidation;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Tests
{
    public class ExpenseServiceTest
    {
        private readonly ExpenseService _sut;
        private readonly Faker _faker;
        private IValidator<Expense> _validator;
        private IValidator<ExpenseFilter> _validatorFilter;
        private readonly Mock<IExpenseRepository> _mockRepository;

        public ExpenseServiceTest()
        {
            _validator = new ExpenseValidator();
            _validatorFilter = new ExpenseFilterValidator();
            _faker = new Faker();
            _mockRepository = new Mock<IExpenseRepository>();
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
        public async Task CreateExpense_MustReturnError_WhenInvalidProperty(Expense bill, string errorMessage)
        {
            // Act & Assert
            var result = await _sut.CreateExpenseAsync(bill);

            Assert.False(result.Success);
            Assert.Contains(errorMessage, result.Errors);
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
                Currency = Currency.Real,
                Type = ExpenseType.Food,
                Month = PurchaseMonth.February
            };

            var filterExpression = ExpensePredicateBuilder.Build(expenseFilter);

            // Act & Assert
            Assert.True((await _sut.GetExpensesWithFilterAsync(expenseFilter)).Success);
            Assert.NotNull(filterExpression);

            _mockRepository.Verify(x => x.GetExpensesWithFilterAsync(
                It.IsAny<Expression<Func<Expense, bool>>>()), Times.AtLeastOnce);
        }

        [Theory]
        [MemberData(nameof(ExpenseFilterData.GetInvalidFilters), MemberType = typeof(ExpenseFilterData))]
        public async Task GetExpensesWithFilter_MustReturnError_WhenInvalidFilter(ExpenseFilter expenseFilter, string errorMessage)
        {
            // Act
            var result = await _sut.GetExpensesWithFilterAsync(expenseFilter);

            // Assert
            Assert.False(result.Success);
            Assert.Contains(errorMessage, result.Errors);
        }

        [Fact]
        public async Task UpdateExpense_MustUpdateSuccesfully()
        {
            // Arrange
            var expense = new Expense
            {
                Id = new Guid(),
                Name = "Almoço",
                Currency = Currency.Euro,
                Value = 1000,
                Type = ExpenseType.Food,
                ExpenseDate = DateTime.Now,
                Source = "Batel Grill"
            };

            _mockRepository.Setup(x => x.GetByIdAsync(expense.Id)).ReturnsAsync(expense);

            // Act & Assert
            Assert.True((await _sut.UpdateExpenseAsync(expense)).Success);
            _mockRepository.Verify(x => x.UpdateAsync(expense), Times.Once);
        }

        [Theory]
        [MemberData(nameof(ExpenseData.GetInvalidExpenses), MemberType = typeof(ExpenseData))]
        public async Task UpdateExpense_MustReturnError_WhenInvalidProperty(Expense expense, string errorMessage)
        {
            // Arrange
            _mockRepository.Setup(x => x.GetByIdAsync(expense.Id)).ReturnsAsync(expense);

            // Act
            var result = await _sut.UpdateExpenseAsync(expense);

            // Assert
            Assert.False(result.Success);
            Assert.Contains(errorMessage, result.Errors);
        }

        [Fact]
        public async Task UpdateExpense_MustReturnError_WhenExpenseNotFound()
        {
            // Arrange
            var expense = new Expense
            {
                Id = new Guid(),
                Name = "Lunch",
                Currency = Currency.Euro,
                Value = 1000,
                Type = ExpenseType.Food,
                ExpenseDate = DateTime.Now,
                Source = "Batel Grill"
            };

            // Act
            var result = await _sut.UpdateExpenseAsync(expense);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Expense not found", result.Errors);
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
            Assert.Contains("ID does not match any expense", result.Errors);
        }

        [Fact]
        public async Task List_MustCallRepository()
        {
            // Act & Assert
            Assert.NotNull(await _sut.GetAllAsync());
        }
    }
}