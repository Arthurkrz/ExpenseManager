using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;

namespace ExpenseManager.Tests.Integration.Builders
{
    public class ExpenseFilterBuilder
    {
        private string? _nameContains;
        private bool _withNameContains;

        private string? _sourceContains;
        private bool _withSourceContains;

        private decimal? _valueStart;
        private bool _withValueStart;

        private decimal? _valueEnd;
        private bool _withValueEnd;

        private DateTime? _dateStart;
        private bool _withDateStart;

        private DateTime? _dateEnd;
        private bool _withDateEnd;

        private Currency? _currency;
        private bool _withCurrency;

        private ExpenseType? _type;
        private bool _withType;

        private PurchaseMonth? _month;
        private bool _withMonth;

        private int? _pageNumber;
        private bool _withPageNumber;

        private int? _pageSize;
        private bool _withPageSize;

        public static ExpenseFilterBuilder Create() => 
            new ExpenseFilterBuilder();

        public ExpenseFilterBuilder WithAllProperties() =>
            WithNameContains().WithSourceContains()
            .WithValueRangeStart().WithValueRangeEnd()
            .WithDateRangeStart().WithDateRangeEnd()
            .WithCurrency().WithType().WithMonth()
            .WithPageNumber().WithPageSize();

        public ExpenseFilterBuilder WithNameContains(string? nameContains = null)
        {
            _nameContains = nameContains;
            _withNameContains = true;

            return this;
        }

        public ExpenseFilterBuilder WithSourceContains(string? sourceContains = null)
        {
            _sourceContains = sourceContains;
            _withSourceContains = true;

            return this;
        }

        public ExpenseFilterBuilder WithValueRangeStart(decimal? valueStart = null)
        {
            _valueStart = valueStart;
            _withValueStart = true;

            return this;
        }

        public ExpenseFilterBuilder WithValueRangeEnd(decimal? valueEnd = null)
        {
            _valueEnd = valueEnd;
            _withValueEnd = true;

            return this;
        }

        public ExpenseFilterBuilder WithDateRangeStart(DateTime? dateStart = null)
        {
            _dateStart = dateStart;
            _withDateStart = true;

            return this;
        }

        public ExpenseFilterBuilder WithDateRangeEnd(DateTime? dateEnd = null)
        {
            _dateEnd = dateEnd;
            _withDateEnd = true;

            return this;
        }

        public ExpenseFilterBuilder WithCurrency(Currency? currency = null)
        {
            _currency = currency;
            _withCurrency = true;

            return this;
        }

        public ExpenseFilterBuilder WithType(ExpenseType? type = null)
        {
            _type = type;
            _withType = true;

            return this;
        }

        public ExpenseFilterBuilder WithMonth(PurchaseMonth? month = null)
        {
            _month = month;
            _withMonth = true;

            return this;
        }

        public ExpenseFilterBuilder WithPageNumber(int? pageNumber = null)
        {
            _pageNumber = pageNumber;
            _withPageNumber = true;

            return this;
        }

        public ExpenseFilterBuilder WithPageSize(int? pageSize = null)
        {
            _pageSize = pageSize;
            _withPageSize = true;

            return this;
        }

        public ExpenseFilter Build() => BuildMany(1).Single();

        public List<ExpenseFilter> BuildMany(int amount)
        {
            var filters = new List<ExpenseFilter>();

            var currencies = Enum.GetValues<Currency>();
            var types = Enum.GetValues<ExpenseType>();
            var months = Enum.GetValues<PurchaseMonth>();

            for (int i = 0; i < amount; i++)
            {
                filters.Add(new ExpenseFilter
                {
                    NameContains = _withNameContains
                        ? _nameContains ?? $"Name{i}"
                        : null,

                    SourceContains = _withSourceContains 
                        ? _sourceContains ?? $"Source{i}" 
                        : null,

                    ValueRangeStart = _withValueStart 
                        ? _valueStart ?? i 
                        : null,
                    
                    ValueRangeEnd = _withValueEnd 
                        ? _valueEnd ?? i + 1 
                        : null,

                    DateRangeStart = _withDateStart 
                        ? _dateStart ?? new DateTime(2026, 08, 20).AddDays(-(i + 1)).Date 
                        : null,

                    DateRangeEnd = _withDateEnd 
                        ? _dateEnd ?? new DateTime(2026, 08, 20).Date 
                        : null,

                    Currency = _withCurrency 
                        ? _currency ?? currencies[i % currencies.Length] 
                        : null,

                    Type = _withType 
                        ? _type ?? types[i % types.Length] 
                        : null,

                    Month = _withMonth
                        ? _month ?? months[i % months.Length]
                        : null,

                    PageNumber = _pageNumber ?? 1,

                    PageSize = _pageSize ?? 100
                });
            }

            return filters;
        }
    }
}
