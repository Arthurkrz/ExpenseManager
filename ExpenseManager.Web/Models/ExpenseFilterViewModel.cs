using ExpenseManager.Web.Models.Enum;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExpenseManager.Web.Models
{
    public class ExpenseFilterViewModel
    {
        public string NameContains { get; set; }

        public string SourceContains { get; set; }

        public decimal? ValueRangeStart
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ValueStringRangeStart)) 
                    return null;

                string valueStringStart = ValueStringRangeStart
                                          .Replace(',', '.');

                decimal.TryParse(valueStringStart,
                    NumberStyles.Currency,
                    CultureInfo.InvariantCulture,
                    out var valueStart);

                return valueStart;
            }
        }

        public decimal? ValueRangeEnd
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ValueStringRangeEnd))
                    return null;

                string valueStringEnd = ValueStringRangeEnd
                                        .Replace(',', '.');

                decimal.TryParse(valueStringEnd,
                    NumberStyles.Currency,
                    CultureInfo.InvariantCulture,
                    out var valueEnd);

                return valueEnd;
            }
        }

        public string ValueStringRangeStart { get; set; }

        public string ValueStringRangeEnd { get; set; }

        public DateTime? DateRangeStart { get; set; }

        public DateTime? DateRangeEnd { get; set; }

        public CurrencyVM? Currency { get; set; }

        public ExpenseTypeVM? Type { get; set; }

        public PurchaseMonthVM? Month { get; set; }

        public List<ExpenseViewModel> Expenses { get; set; } = [];

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}