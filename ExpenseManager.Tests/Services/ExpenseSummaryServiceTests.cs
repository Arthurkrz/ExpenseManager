using ExpenseManager.Core.Common;
using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Entities;
using ExpenseManager.Service;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace ExpenseManager.Tests.Services
{
    public class ExpenseSummaryServiceTests
    {
        private readonly Mock<IExpenseSummaryRepository> _repositoryMock = new();
        private readonly Mock<IValidator<ExpenseFilter>> _validatorMock = new();
        private readonly ExpenseSummaryService _sut;

        public ExpenseSummaryServiceTests()
        {
            _sut = new(_repositoryMock.Object, _validatorMock.Object);
        }

        [Fact]
        public async Task GetTotalsByCurrencyAsync_MustCallRepositoryMethod()
        {
            // Act
            await _sut.GetTotalsByCurrencyAsync();

            // Assert
            _repositoryMock.Verify(x => 
                x.GetTotalsByCurrencyAsync(), Times.Once);
        }

        [Fact]
        public async Task GetTotalsByCurrencyFilterAsync_MustReturnError_WhenInvalidFilter()
        {
            // Arrange
            var filter = new ExpenseFilter
            {
                NameContains = "NameContains",
                SourceContains = "SourceContains",
                PageNumber = 1,
                PageSize = 1
            };

            var validationResult = new ValidationResult 
            { Errors = [new ValidationFailure { ErrorMessage = "Error" }] };

            _validatorMock.Setup(x => x.Validate(filter))
                .Returns(validationResult);

            // Act
            var result = await _sut.GetTotalsByCurrencyFilterAsync(filter);

            // Assert
            Assert.False(result.Success);
            Assert.Contains("Error", result.Errors);
        }

        [Fact]
        public async Task GetTotalsByCurrencyFilterAsync_MustGetSuccessfully()
        {
            // Arrange
            var filter = new ExpenseFilter
            {
                NameContains = "NameContains",
                SourceContains = "SourceContains",
                PageNumber = 1,
                PageSize = 1
            };

            _validatorMock.Setup(x => x.Validate(filter))
                .Returns(new ValidationResult());

            _repositoryMock.Setup(x => x.GetTotalsByCurrencyFilterAsync(
                It.IsAny<Expression<Func<Expense, bool>>>()))
                .ReturnsAsync(new ExpenseSummaryResult());

            // Act
            var result = await _sut.GetTotalsByCurrencyFilterAsync(filter);

            // Assert
            _repositoryMock.Verify(x =>
                x.GetTotalsByCurrencyFilterAsync(
                    It.IsAny<Expression<Func<Expense, bool>>>()), 
                    Times.Once);

            Assert.True(result.Success);
            Assert.NotNull(result.Data);
        }
    }
}
