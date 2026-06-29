using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using ExpenseManager.Web.Models;
using ExpenseManager.Web.Models.Enum;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Tests.ObjectGenerators
{
    public class MappingData
    {
        public static IEnumerable<object[]> GetValidObjects()
        {
            yield return new object[]
            {
                new Expense()
                {
                    Id = new Guid(),
                    Name = "Televisão",
                    Currency = Currency.Euro,
                    Value = 1000,
                    Type = ExpenseType.House,
                    ExpenseDate = DateTime.Now.AddMonths(-1).Date,
                    Source = "Casas Bahia"
                },

                new ExpenseViewModel()
                {
                    Id = new Guid(),
                    Name = "Televisão",
                    Currency = CurrencyVM.Euro,
                    ValueString = "1000.00",
                    Type = ExpenseTypeVM.House,
                    ExpenseDate = DateTime.Now.AddMonths(-1).Date,
                    Source = "Casas Bahia"
                }
            };

            yield return new object[]
            {
                new ExpenseViewModel()
                {
                    Id = new Guid(),
                    Name = "Televisão",
                    Currency = CurrencyVM.Euro,
                    ValueString = "1000.00",
                    Type = ExpenseTypeVM.House,
                    ExpenseDate = DateTime.Now.AddMonths(-1).Date,
                    Source = "Casas Bahia"
                },

                new Expense()
                {
                    Id = new Guid(),
                    Name = "Televisão",
                    Currency = Currency.Euro,
                    Value = 1000,
                    Type = ExpenseType.House,
                    ExpenseDate = DateTime.Now.AddMonths(-1).Date,
                    Source = "Casas Bahia"
                }
            };

            yield return new object[]
            {
                new ExpenseFilter()
                {
                    NameContains = "Motocicleta",
                    SourceContains = "Yamaha",
                    ValueRangeStart = 9000,
                    ValueRangeEnd = 20000,
                    DateRangeStart = DateTime.Now.AddMonths(-6).Date,
                    DateRangeEnd = DateTime.Now.Date,
                    Currency = Currency.Real,
                    Type = ExpenseType.Transport,
                    Month = PurchaseMonth.July
                },

                new ExpenseFilterViewModel()
                {
                    NameContains = "Motocicleta",
                    SourceContains = "Yamaha",
                    ValueStringRangeStart = "9000.00",
                    ValueStringRangeEnd = "20000.00",
                    DateRangeStart = DateTime.Now.AddMonths(-6).Date,
                    DateRangeEnd = DateTime.Now.Date,
                    Currency = CurrencyVM.Real,
                    Type = ExpenseTypeVM.Transport,
                    Month = PurchaseMonthVM.July,
                }
            };

            yield return new object[]
            {
                new ExpenseFilterViewModel()
                {
                    NameContains = "Motocicleta",
                    SourceContains = "Yamaha",
                    ValueStringRangeStart = "9000.00",
                    ValueStringRangeEnd = "20000.00",
                    DateRangeStart = DateTime.Now.AddMonths(-6).Date,
                    DateRangeEnd = DateTime.Now.Date,
                    Currency = CurrencyVM.Real,
                    Type = ExpenseTypeVM.Transport,
                    Month = PurchaseMonthVM.July,
                },

                new ExpenseFilter()
                {
                    NameContains = "Motocicleta",
                    SourceContains = "Yamaha",
                    ValueRangeStart = 9000,
                    ValueRangeEnd = 20000,
                    DateRangeStart = DateTime.Now.AddMonths(-6).Date,
                    DateRangeEnd = DateTime.Now.Date,
                    Currency = Currency.Real,
                    Type = ExpenseType.Transport,
                    Month = PurchaseMonth.July
                }
            };
        }
    }
}
