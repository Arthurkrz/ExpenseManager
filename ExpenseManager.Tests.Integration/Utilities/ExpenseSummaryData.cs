using Bogus;
using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using ExpenseManager.Tests.Integration.Builders;

namespace ExpenseManager.Tests.Integration.Utilities
{
    public static class ExpenseSummaryData
    {
        public static IEnumerable<object[]> GetSummaries()
        {
            var _faker = new Faker();

            var expenses = new List<Expense>();

            var values = new List<decimal>
            {
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000)
            };

            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[0]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[1]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[2]).WithType().WithDate().WithSource().Build());

            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[3]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[4]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[5]).WithType().WithDate().WithSource().Build());

            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[6]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[7]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[8]).WithType().WithDate().WithSource().Build());

            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.GBP).WithValue(values[9]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.GBP).WithValue(values[10]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.GBP).WithValue(values[11]).WithType().WithDate().WithSource().Build());

            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[12]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[13]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[14]).WithType().WithDate().WithSource().Build());

            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[15]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[16]).WithType().WithDate().WithSource().Build());
            expenses.Add(ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[17]).WithType().WithDate().WithSource().Build());

            yield return new object[]
            {
                expenses,

                new Dictionary<string, decimal>
                {
                    { "USD", values[0] + values[1] + values[2] },

                    { "EUR", values[3] + values[4] + values[5] },

                    { "BRL", values[6] + values[7] + values[8] },

                    { "GBP", values[9] + values[10] + values[11] },

                    { "CAD", values[12] + values[13] + values[14] },

                    { "JPY", values[15] + values[16] + values[17] }
                }
            };
        }

        public static IEnumerable<object[]> GetFilteredSummaries()
        {
            var _faker = new Faker();

            var values = new List<decimal>
            {
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),
                _faker.Finance.Amount(1, 1000000),

                _faker.Finance.Amount(1, 1000000)
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[0]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[1]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[2]).WithType().WithDate().WithSource().Build(),

                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[18]).WithType().WithDate().WithSource().Build()
                },

                new ExpenseFilter { Currency = Currency.USD },

                new Dictionary<string, decimal> { { "USD", values[0] + values[1] + values[2] } }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[3]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[4]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[5]).WithType().WithDate().WithSource().Build(),

                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.GBP).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[18]).WithType().WithDate().WithSource().Build()
                },

                new ExpenseFilter { Currency = Currency.EUR },

                new Dictionary<string, decimal> { { "EUR", values[3] + values[4] + values[5] } }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[6]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[7]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[8]).WithType().WithDate().WithSource().Build(),

                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.GBP).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[18]).WithType().WithDate().WithSource().Build()
                },

                new ExpenseFilter { Currency = Currency.BRL },

                new Dictionary<string, decimal> { { "BRL", values[6] + values[7] + values[8] } }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.GBP).WithValue(values[9]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.GBP).WithValue(values[10]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.GBP).WithValue(values[11]).WithType().WithDate().WithSource().Build(),

                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[18]).WithType().WithDate().WithSource().Build()
                },

                new ExpenseFilter { Currency = Currency.GBP },

                new Dictionary<string, decimal> { { "GBP", values[9] + values[10] + values[11] } }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[12]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[13]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.CAD).WithValue(values[14]).WithType().WithDate().WithSource().Build(),

                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[18]).WithType().WithDate().WithSource().Build()
                },

                new ExpenseFilter { Currency = Currency.CAD },

                new Dictionary<string, decimal> { { "CAD", values[12] + values[13] + values[14] } }
            };

            yield return new object[]
            {
                new List<Expense>
                {
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[15]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[16]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.JPY).WithValue(values[17]).WithType().WithDate().WithSource().Build(),

                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.USD).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.EUR).WithValue(values[18]).WithType().WithDate().WithSource().Build(),
                    ExpenseBuilder.Create().WithId().WithName().WithCurrency(Currency.BRL).WithValue(values[18]).WithType().WithDate().WithSource().Build()
                },

                new ExpenseFilter { Currency = Currency.JPY },

                new Dictionary<string, decimal> { { "JPY", values[15] + values[16] + values[17] } }
            };
        }
    }
}
