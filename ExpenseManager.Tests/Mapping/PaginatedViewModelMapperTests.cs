using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using ExpenseManager.Web.Mapping;
using ExpenseManager.Web.Mapping.Configurations;
using ExpenseManager.Web.Mapping.Contracts;
using ExpenseManager.Web.Models;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace ExpenseManager.Tests.Mapping
{
    public class PaginatedViewModelMapperTests
    {
        [Fact]
        public void ToExpensePaginatedViewModel_ShouldMap()
        {
            // Arrange
            var objectMapper = new ObjectMapper();

            var sut = new PaginatedViewModelMapper(objectMapper);

            var expense = new Expense
            {
                Id = Guid.NewGuid(),
                Name = "Name",
                Currency = Currency.BRL,
                Value = 119.9m,
                Type = ExpenseType.Food,
                Source = "Source",
                ExpenseDate = new DateTime(2026, 7, 10)
            };

            var source = new PaginatedResult<Expense>
            {
                Items = [expense],
                PageNumber = 2,
                PageSize = 10,
                TotalCount = 25
            };

            // Act
            var result = sut.ToExpensePaginatedViewModel(source);

            // Assert
            var item = Assert.Single(result.Items);
            item.Should().BeEquivalentTo(expense, options => options.ExcludingMissingMembers());
        }

        [Fact]
        public void ToExpensePaginatedViewModel_ShouldDelegateItemMappingToObjectMapper()
        {
            // Arrange
            var expenses = new List<Expense> { new(), new() };

            var mappedExpenses = new List<ExpenseViewModel>
            {
                new() { Name = "First" },
                new() { Name = "Second" }
            };

            var mapperMock = new Mock<IObjectMapper>();

            mapperMock.Setup(m => m.MapCollection(expenses, 
                It.IsAny<Action<MappingOptions<Expense, ExpenseViewModel>>>()))
                    .Returns(mappedExpenses);

            var sut = new PaginatedViewModelMapper(mapperMock.Object);

            var source = new PaginatedResult<Expense>
            {
                Items = expenses,
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 2
            };

            // Act
            var result = sut.ToExpensePaginatedViewModel(source);

            // Assert
            Assert.Equal(mappedExpenses, result.Items);

            mapperMock.Verify(m => m.MapCollection(expenses, 
                It.IsAny<Action<MappingOptions<Expense, ExpenseViewModel>>>()), 
                    Times.Once);
        }

        [Fact]
        public void ToExpensePaginatedViewModel_ShouldReturnEmptyItems_WhenPageHasNoExpenses()
        {
            // Arrange
            var sut = new PaginatedViewModelMapper(new ObjectMapper());
            
            var source = new PaginatedResult<Expense>
            {
                Items = [],
                PageNumber = 1,
                PageSize = 10,
                TotalCount = 0
            };

            // Act
            var result = sut.ToExpensePaginatedViewModel(source);

            // Assert
            Assert.Empty(result.Items);
            Assert.Equal(1, result.PageNumber);
            Assert.False(result.HasPreviousPage);
            Assert.False(result.HasNextPage);
        }
    }
}
