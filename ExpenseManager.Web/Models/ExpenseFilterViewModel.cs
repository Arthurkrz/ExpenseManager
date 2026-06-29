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

        public double? ValueRangeStart
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ValueStringRangeStart)) 
                    return null;

                string valueStringStart = ValueStringRangeStart
                                          .Replace(',', '.');

                double.TryParse(valueStringStart,
                                NumberStyles.Currency,
                                CultureInfo.InvariantCulture,
                                out var valueStart);

                return valueStart;
            }
        }

        public string ValueStringRangeStart { get; set; }

        public string ValueStringRangeEnd { get; set; }

        public double? ValueRangeEnd
        {
            get
            {
                if (string.IsNullOrWhiteSpace(ValueStringRangeEnd))
                    return null;

                string valueStringEnd = ValueStringRangeEnd
                                        .Replace(',', '.');

                double.TryParse(valueStringEnd,
                                NumberStyles.Currency,
                                CultureInfo.InvariantCulture,
                                out var valueEnd);

                return valueEnd;
            }
        }

        public DateTime? DateRangeStart { get; set; }

        public DateTime? DateRangeEnd { get; set; }

        public CurrencyVM? Currency { get; set; }

        public ExpenseTypeVM? Type { get; set; }

        public PurchaseMonthVM? Month { get; set; }

        public List<ExpenseViewModel> Expenses { get; set; } = [];
    }
}