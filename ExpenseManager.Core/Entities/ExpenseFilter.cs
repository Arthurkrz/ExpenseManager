using ExpenseManager.Core.Enum;
using System;

namespace ExpenseManager.Core.Entities
{
    public class ExpenseFilter
    {
        public string NameContains { get; set; }
        
        public string SourceContains { get; set; }
        
        public decimal? ValueRangeStart { get; set; }
        
        public decimal? ValueRangeEnd { get; set; }
        
        public DateTime? DateRangeStart { get; set; }
        
        public DateTime? DateRangeEnd { get; set; }
        
        public Currency? Currency { get; set; }
        
        public ExpenseType? Type { get; set; }
        
        public PurchaseMonth? Month { get; set; }
    }
}