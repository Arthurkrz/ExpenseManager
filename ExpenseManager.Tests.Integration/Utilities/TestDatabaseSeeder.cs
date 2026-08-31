using ExpenseManager.Core.Entities;
using ExpenseManager.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseManager.Tests.Integration.Utilities
{
    public static class TestDatabaseSeeder
    {
        public static async Task InsertExpensesAsync(IServiceProvider services, List<Expense> expenses)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<Context>();

            await context.Expenses.AddRangeAsync(expenses);
            await context.SaveChangesAsync();
        }
    }
}
