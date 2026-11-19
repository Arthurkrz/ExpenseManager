using ExpenseManager.Core.Entities;

namespace ExpenseManager.Tests.Integration.Utilities
{
    public class ExpenseTableRow
    {
        public Guid Id { get; set; } = Guid.Empty;
        
        public string Name { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public string Value { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Source { get; set; } = string.Empty;

        public string ExpenseDate { get; set; } = string.Empty;

        public static ExpenseTableRow FromExpense(Expense expense) =>
            new ExpenseTableRow
            {
                Id = expense.Id,
                Name = expense.Name,
                Currency = expense.Currency.ToString()!,
                Value = expense.Value.ToString("F2"),
                Type = expense.Type.ToString()!,
                Source = expense.Source,
                ExpenseDate = expense.ExpenseDate.ToString("yyyy-MM-dd")
            };
    }
}
