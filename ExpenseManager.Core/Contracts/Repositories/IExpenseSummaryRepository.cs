using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Repositories
{
    public interface IExpenseSummaryRepository
    {
        Task<ExpenseSummaryResult> GetTotalsByCurrencyAsync();

        Task<ExpenseSummaryResult> GetTotalsByCurrencyFilterAsync(Expression<Func<Expense, bool>> predicate);
    }
}
