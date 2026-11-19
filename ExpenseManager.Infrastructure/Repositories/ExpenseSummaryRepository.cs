using ExpenseManager.Core.Common;
using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseManager.Infrastructure.Repositories
{
    public class ExpenseSummaryRepository : IExpenseSummaryRepository
    {
        private readonly Context _context;

        public ExpenseSummaryRepository(Context context)
        {
            _context = context;
        }

        public async Task<ExpenseSummaryResult> GetTotalsByCurrencyAsync()
        {
            var totals = await _context.Set<Expense>().AsNoTracking()
            .GroupBy(e => e.Currency).Select(group => new
            {
                Currency = group.Key.ToString(),
                Total = group.Sum(e => e.Value)
            })
            .ToDictionaryAsync(x => x.Currency, x => x.Total);
            
            return new ExpenseSummaryResult
            { TotalsByCurrency = totals };

        }

        public async Task<ExpenseSummaryResult> GetTotalsByCurrencyFilterAsync(Expression<Func<Expense, bool>> predicate)
        {
            var totals = await _context.Set<Expense>()
            .AsNoTracking().Where(predicate)
            .GroupBy(e => e.Currency).Select(group => new
            {
                Currency = group.Key.ToString(),
                Total = group.Sum(expense => expense.Value)
            })
            .ToDictionaryAsync(x => x.Currency, x => x.Total);

            return new ExpenseSummaryResult
            { TotalsByCurrency = totals };
        }
    }
}
