using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using ExpenseManager.Tests.Integration.Builders;

namespace ExpenseManager.Tests.Integration.Utilities
{
    public static class ExpenseFilterData
    {
        public static IEnumerable<object[]> GetFiltersAll()
        {
            var expenses = ExpenseBuilder.Create().WithAllProperties().BuildMany(20);

            var expensesInPage = new List<Expense>
            {
                new Expense { Name = "Name11", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                new Expense { Name = "Name12", Currency = Currency.CAD, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" },
                new Expense { Name = "Name13", Currency = Currency.JPY, Value = 13, Type = ExpenseType.Fun, ExpenseDate = new DateTime(2026, 08, 2).Date, Source = "Source13" },
                new Expense { Name = "Name14", Currency = Currency.USD, Value = 14, Type = ExpenseType.Services, ExpenseDate = new DateTime(2026, 08, 1).Date, Source = "Source14" },
                new Expense { Name = "Name15", Currency = Currency.EUR, Value = 15, Type = ExpenseType.Electronics, ExpenseDate = new DateTime(2026, 07, 31).Date, Source = "Source15" }
            };

            yield return new object[]
            {
                expenses,

                new ExpenseFilter { PageNumber = 3, PageSize = 5 },

                new PaginatedResult<Expense>
                {
                    Items = expensesInPage,

                    PageNumber = 3,

                    PageSize = 5,

                    TotalCount = 20
                }
            };
        }

        public static IEnumerable<object[]> GetFiltersEach()
        {
            yield return new object[]
            {
                new List<Expense>
                {
                    new Expense { Name = "Name", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                    new Expense { Name = "Name", Currency = Currency.CAD, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" },

                    new Expense { Name = "Name1", Currency = Currency.JPY, Value = 13, Type = ExpenseType.Fun, ExpenseDate = new DateTime(2026, 08, 2).Date, Source = "Source13" },
                    new Expense { Name = "Name2", Currency = Currency.USD, Value = 14, Type = ExpenseType.Services, ExpenseDate = new DateTime(2026, 08, 1).Date, Source = "Source14" }
                },

                new ExpenseFilter { NameContains = "Name" },

                new PaginatedResult<Expense>
                {
                    Items = new List<Expense>
                    {
                        new Expense { Name = "Name", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                        new Expense { Name = "Name", Currency = Currency.CAD, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" }
                    },

                    PageNumber = 1,

                    PageSize = 5,

                    TotalCount = 2
                }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    new Expense { Name = "Name11", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source" },
                    new Expense { Name = "Name12", Currency = Currency.CAD, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source" },

                    new Expense { Name = "Name13", Currency = Currency.JPY, Value = 13, Type = ExpenseType.Fun, ExpenseDate = new DateTime(2026, 08, 2).Date, Source = "Source1" },
                    new Expense { Name = "Name14", Currency = Currency.USD, Value = 14, Type = ExpenseType.Services, ExpenseDate = new DateTime(2026, 08, 1).Date, Source = "Source2" }
                },

                new ExpenseFilter { SourceContains = "Source" },

                new PaginatedResult<Expense>
                {
                    Items = new List<Expense>
                    {
                        new Expense { Name = "Name11", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source" },
                        new Expense { Name = "Name12", Currency = Currency.CAD, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source" }
                    },

                    PageNumber = 1,

                    PageSize = 5,

                    TotalCount = 2
                }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    new Expense { Name = "Name11", Currency = Currency.GBP, Value = 15, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                    new Expense { Name = "Name12", Currency = Currency.CAD, Value = 15, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" },

                    new Expense { Name = "Name13", Currency = Currency.JPY, Value = 13, Type = ExpenseType.Fun, ExpenseDate = new DateTime(2026, 08, 2).Date, Source = "Source13" },
                    new Expense { Name = "Name14", Currency = Currency.USD, Value = 14, Type = ExpenseType.Services, ExpenseDate = new DateTime(2026, 08, 1).Date, Source = "Source14" }
                },

                new ExpenseFilter { ValueRangeStart = 15, ValueRangeEnd = 15 },

                new PaginatedResult<Expense>
                {
                    Items = new List<Expense>
                    {
                        new Expense { Name = "Name11", Currency = Currency.GBP, Value = 15, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                        new Expense { Name = "Name12", Currency = Currency.CAD, Value = 15, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" },
                    },

                    PageNumber = 1,

                    PageSize = 5,

                    TotalCount = 2
                }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    new Expense { Name = "Name11", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 20).Date, Source = "Source11" },
                    new Expense { Name = "Name12", Currency = Currency.CAD, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 20).Date, Source = "Source12" },

                    new Expense { Name = "Name13", Currency = Currency.JPY, Value = 13, Type = ExpenseType.Fun, ExpenseDate = new DateTime(2026, 08, 2).Date, Source = "Source13" },
                    new Expense { Name = "Name14", Currency = Currency.USD, Value = 14, Type = ExpenseType.Services, ExpenseDate = new DateTime(2026, 08, 1).Date, Source = "Source14" }
                },

                new ExpenseFilter { DateRangeStart = new DateTime(2026, 08, 20), DateRangeEnd = new DateTime(2026, 08, 20) },

                new PaginatedResult<Expense>
                {
                    Items = new List<Expense>
                    {
                        new Expense { Name = "Name11", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 20).Date, Source = "Source11" },
                        new Expense { Name = "Name12", Currency = Currency.CAD, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 20).Date, Source = "Source12" },
                    },

                    PageNumber = 1,

                    PageSize = 5,

                    TotalCount = 2
                }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    new Expense { Name = "Name11", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                    new Expense { Name = "Name12", Currency = Currency.CAD, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" },

                    new Expense { Name = "Name13", Currency = Currency.JPY, Value = 13, Type = ExpenseType.Fun, ExpenseDate = new DateTime(2026, 08, 2).Date, Source = "Source13" },
                    new Expense { Name = "Name14", Currency = Currency.USD, Value = 14, Type = ExpenseType.Services, ExpenseDate = new DateTime(2026, 08, 1).Date, Source = "Source14" }
                },

                new ExpenseFilter { Currency = Currency.BRL },

                new PaginatedResult<Expense>
                {
                    Items = new List<Expense>
                    {
                        new Expense { Name = "Name11", Currency = Currency.BRL, Value = 11, Type = ExpenseType.Transport, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                        new Expense { Name = "Name12", Currency = Currency.BRL, Value = 12, Type = ExpenseType.House, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" },
                    },

                    PageNumber = 1,

                    PageSize = 5,

                    TotalCount = 2
                }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    new Expense { Name = "Name11", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Debts, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                    new Expense { Name = "Name12", Currency = Currency.CAD, Value = 12, Type = ExpenseType.Debts, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" },

                    new Expense { Name = "Name13", Currency = Currency.JPY, Value = 13, Type = ExpenseType.Fun, ExpenseDate = new DateTime(2026, 08, 2).Date, Source = "Source13" },
                    new Expense { Name = "Name14", Currency = Currency.USD, Value = 14, Type = ExpenseType.Services, ExpenseDate = new DateTime(2026, 08, 1).Date, Source = "Source14" }
                },

                new ExpenseFilter { Type = ExpenseType.Debts },

                new PaginatedResult<Expense>
                {
                    Items = new List<Expense>
                    {
                        new Expense { Name = "Name11", Currency = Currency.GBP, Value = 11, Type = ExpenseType.Debts, ExpenseDate = new DateTime(2026, 08, 4).Date, Source = "Source11" },
                        new Expense { Name = "Name12", Currency = Currency.CAD, Value = 12, Type = ExpenseType.Debts, ExpenseDate = new DateTime(2026, 08, 3).Date, Source = "Source12" },
                    },

                    PageNumber = 1,

                    PageSize = 5,

                    TotalCount = 2
                }
            };
        }

        public static IEnumerable<object[]> GetFiltersOne()
        {
            var expenses = ExpenseBuilder.Create().WithAllProperties().BuildMany(2);

            var expense = ExpenseBuilder.Create().WithId()
                .WithName("UniqueName").WithCurrency(Currency.JPY)
                .WithValue(100).WithType(ExpenseType.Miscellaneous)
                .WithDate(new DateTime(2026, 08, 20).Date)
                .WithSource("UniqueSource").Build();

            expenses.Add(expense);

            yield return new object[]
            {
                expenses,

                new ExpenseFilter
                {
                    NameContains = "UniqueName",
                    SourceContains = "UniqueSource",
                    ValueRangeStart = 100,
                    ValueRangeEnd = 100,
                    DateRangeStart = new DateTime(2026, 08, 20).Date,
                    DateRangeEnd = new DateTime(2026, 08, 20).Date,
                    Currency = Currency.JPY,
                    Type = ExpenseType.Miscellaneous,
                    Month = PurchaseMonth.August,
                    PageNumber = 3,
                    PageSize = 3
                },

                new PaginatedResult<Expense>
                {
                    Items = [expense],

                    PageNumber = 3,

                    PageSize = 1,

                    TotalCount = 3
                }
            };
        }

        public static IEnumerable<object[]> GetFiltersNone()
        {
            var expenses = ExpenseBuilder.Create().WithAllProperties().BuildMany(3);

            yield return new object[]
            {
                expenses,

                new ExpenseFilter
                {
                    NameContains = "NameContains",
                    SourceContains = "SourceContains",
                    ValueRangeStart = 100,
                    ValueRangeEnd = 200,
                    DateRangeStart = new DateTime(2024, 1, 1),
                    DateRangeEnd = new DateTime(2024, 12, 31),
                    Currency = Currency.JPY,
                    Type = ExpenseType.Miscellaneous,
                    Month = PurchaseMonth.December,
                    PageNumber = 1,
                    PageSize = 5,
                }
            };
        }
    }
}
