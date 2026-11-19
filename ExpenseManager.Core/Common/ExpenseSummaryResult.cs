using System.Collections.Generic;

namespace ExpenseManager.Core.Common
{
    public class ExpenseSummaryResult
    {
        public Dictionary<string, decimal> TotalsByCurrency { get; set; } = new();
    }
}
