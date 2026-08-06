using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using ExpenseManager.Web.Mapping;
using ExpenseManager.Web.Mapping.CustomMappings;
using ExpenseManager.Web.Models;
using ExpenseManager.Web.Models.Enum;
using System;
using Xunit;

namespace ExpenseManager.Tests.Mapping
{
    public class ExpenseMappingTests
    {
        private readonly ObjectMapper _sut = new();

        [Fact]
        public void MapExpenseToViewModel_ShouldMap()
        {
            // Arrange
            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Currency = Currency.EUR,
                Value = 1000,
                Type = ExpenseType.Electronics,
                Source = "Source",
                ExpenseDate = new DateTime(2026, 7, 20)
            };

            // Act
            var result = _sut.Map<Expense, ExpenseViewModel>(
                expense, options => options.MapExpenseToViewModel());

            // Assert
            Assert.Equal(expense.Id, result.Id);
            Assert.Equal(expense.Name, result.Name);
            Assert.Equal(CurrencyVM.EUR, result.Currency);
            Assert.Equal("1000.00", result.ValueString);
            Assert.Equal(ExpenseTypeVM.Electronics, result.Type);
            Assert.Equal(expense.Source, result.Source);
            Assert.Equal(expense.ExpenseDate, result.ExpenseDate);
        }

        [Fact]
        public void MapExpenseToViewModel_ShouldPreserveNullableEnums()
        {
            // Arrange
            var expense = new Expense
            {
                Currency = null,
                Type = null,
                ExpenseDate = new DateTime(2026, 7, 20)
            };

            // Act
            var result = _sut.Map<Expense, ExpenseViewModel>(
                expense, options => options.MapExpenseToViewModel());

            // Assert
            Assert.Null(result.Currency);
            Assert.Null(result.Type);
        }

        [Fact]
        public void MapViewModelToExpense_ShouldMap()
        {
            // Arrange
            var viewModel = new ExpenseViewModel
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Currency = CurrencyVM.BRL,
                ValueString = "1000.00",
                Type = ExpenseTypeVM.House,
                Source = "Source",
                ExpenseDate = new DateTime(2026, 7, 21)
            };

            // Act
            var result = _sut.Map<ExpenseViewModel, Expense>(
                viewModel, options => options.MapViewModelToExpense());

            // Assert
            Assert.Equal(viewModel.Id, result.Id);
            Assert.Equal(viewModel.Name, result.Name);
            Assert.Equal(Currency.BRL, result.Currency);
            Assert.Equal(1000.00m, result.Value);
            Assert.Equal(ExpenseType.House, result.Type);
            Assert.Equal(viewModel.Source, result.Source);
            Assert.Equal(viewModel.ExpenseDate, result.ExpenseDate);
        }

        [Fact]
        public void MapViewModelToExpense_ShouldThrowException_WhenNullExpenseDate()
        {
            // Arrange
            var viewModel = new ExpenseViewModel
            {
                ValueString = "1000.00",
                ExpenseDate = null
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
                _sut.Map<ExpenseViewModel, Expense>(viewModel,
                    options => options.MapViewModelToExpense()));
        }

        [Fact]
        public void MapViewModelToFilter_ShouldMap()
        {
            // Arrange
            var viewModel = new ExpenseFilterViewModel
            {
                NameContains = "NameContains",
                SourceContains = "SourceContains",
                ValueStringRangeStart = "1000,50",
                ValueStringRangeEnd = "2000.50",
                DateRangeStart = new DateTime(2026, 1, 1),
                DateRangeEnd = new DateTime(2026, 7, 28),
                Currency = CurrencyVM.BRL,
                Type = ExpenseTypeVM.House,
                Month = PurchaseMonthVM.July,
                PageNumber = 3,
                PageSize = 20
            };

            // Act
            var result = _sut.Map<ExpenseFilterViewModel, ExpenseFilter>(
                viewModel, options => options.MapViewModelToFilter());

            // Assert
            Assert.Equal(viewModel.NameContains, result.NameContains);
            Assert.Equal(viewModel.SourceContains, result.SourceContains);
            Assert.Equal(1000.50m, result.ValueRangeStart);
            Assert.Equal(2000.50m, result.ValueRangeEnd);
            Assert.Equal(viewModel.DateRangeStart, result.DateRangeStart);
            Assert.Equal(viewModel.DateRangeEnd, result.DateRangeEnd);
            Assert.Equal(Currency.BRL, result.Currency);
            Assert.Equal(ExpenseType.House, result.Type);
            Assert.Equal(PurchaseMonth.July, result.Month);
            Assert.Equal(viewModel.PageNumber, result.PageNumber);
            Assert.Equal(viewModel.PageSize, result.PageSize);
        }

        [Fact]
        public void MapViewModelToFilter_ShouldThrow_WhenInvalidOrEmptyMoney()
        {
            // Arrange
            var viewModel = new ExpenseFilterViewModel
            {
                ValueStringRangeStart = "",
                ValueStringRangeEnd = "value",
            };

            // Act & Assert
            var exception = Assert.Throws<FormatException>(() => 
                _sut.Map<ExpenseFilterViewModel, ExpenseFilter>(
                viewModel, options => options.MapViewModelToFilter()));

            Assert.Equal("The value 'value' is not " +
                "a valid monetary value.", exception.Message);
        }
    }
}
