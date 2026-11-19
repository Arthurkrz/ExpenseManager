using ExpenseManager.Core.Enum;
using System;

namespace ExpenseManager.Core.Entities
{
    public class Expense : Entity
    {
        public string Name { get; set; }
        
        public Currency? Currency { get; set; }
        
        public decimal Value { get; set; }
        
        public ExpenseType? Type { get; set; }
        
        public DateTime ExpenseDate { get; set; }
        
        public string Source { get; set; }
    }
}