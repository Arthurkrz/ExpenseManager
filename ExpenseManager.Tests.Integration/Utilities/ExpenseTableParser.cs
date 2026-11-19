using AngleSharp;
using AngleSharp.Dom;

namespace ExpenseManager.Tests.Integration.Utilities
{
    public static class ExpenseTableParser
    {
        public static async Task<List<ExpenseTableRow>> ParseAsync(HttpResponseMessage response)
        {
            var html = await response.Content.ReadAsStringAsync();
            var context = BrowsingContext.New(Configuration.Default);

            var document = await context.OpenAsync(
                request => request.Content(html));

            var rows = document.QuerySelectorAll(
                ".expense-table .expenserow");

            var expenses = new List<ExpenseTableRow>();

            foreach (var row in rows)
                expenses.Add(new ExpenseTableRow
                {
                    Id = Guid.TryParse(row.GetAttribute("data-expense-id"),
                        out var id) ? id : Guid.Empty,

                    Name = GetText(row, ".expense-name"),

                    Currency = GetText(row, ".expense-currency"),

                    Value = GetText(row, "Value"),

                    Type = GetText(row, ".expense-type"),

                    Source = GetText(row, ".expense-source"),

                    ExpenseDate = GetText(row, ".expense-date")
                });

            return expenses;
        }

        private static string GetText(IElement row, string selector) => 
            row.QuerySelector(selector)?.TextContent?.Trim() ?? string.Empty;
    }
}
