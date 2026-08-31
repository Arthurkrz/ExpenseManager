using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;

namespace ExpenseManager.Tests.Integration.Builders
{
    public class ExpenseBuilder
    {
        private Guid? _id;

        private string? _name;
        private bool _withName;

        private Currency? _currency;
        private bool _withCurrency;

        private decimal? _value;
        private bool _withValue;

        private ExpenseType? _type;
        private bool _withType;

        private DateTime? _expenseDate;
        private bool _withExpenseDate;

        private string? _source;
        private bool _withSource;

        public static ExpenseBuilder Create() => new ExpenseBuilder();

        public ExpenseBuilder WithAllProperties() =>
            WithId().WithName().WithCurrency().WithValue()
            .WithType().WithDate().WithSource();

        public ExpenseBuilder WithId(Guid? id = null)
        {
            _id = id;

            return this;
        }

        public ExpenseBuilder WithName(string? name = null)
        {
            _name = name;
            _withName = true;

            return this;
        }

        public ExpenseBuilder WithCurrency(Currency? currency = null)
        {
            _currency = currency;
            _withCurrency = true;

            return this;
        }

        public ExpenseBuilder WithValue(decimal? value = null)
        {
            _value = value;
            _withValue = true;

            return this;
        }

        public ExpenseBuilder WithType(ExpenseType? type = null)
        {
            _type = type;
            _withType = true;

            return this;
        }

        public ExpenseBuilder WithDate(DateTime? expenseDate = null)
        {
            _expenseDate = expenseDate;
            _withExpenseDate = true;

            return this;
        }

        public ExpenseBuilder WithSource(string? source = null!)
        {
            _source = source;
            _withSource = true;

            return this;
        }

        public Expense Build() => BuildMany(1).Single();

        public List<Expense> BuildMany(int amount)
        {
            var expenses = new List<Expense>();

            var currencies = Enum.GetValues<Currency>();
            var types = Enum.GetValues<ExpenseType>();

            for (int i = 0; i < amount; i++)
            {
                expenses.Add(new Expense
                {
                    Id = _id ?? default,

                    Name = _withName
                        ? _name ?? $"Name{i + 1}"
                        : null,

                    Currency = _withCurrency
                        ? _currency ?? currencies[i % currencies.Length]
                        : null,

                    Value = _withValue
                        ? _value ?? i + 1
                        : default,

                    Type = _withType
                        ? _type ?? types[i % types.Length]
                        : null,

                    ExpenseDate = _withExpenseDate
                        ? (_expenseDate?.Date ?? new DateTime(2026, 08, 15)
                            .AddDays(-(i + 1)).Date)
                        : DateTime.MinValue.Date,

                    Source = _withSource
                        ? _source ?? $"Source{i + 1}"
                        : null
                });
            }

            return expenses;
        }
    }
}
