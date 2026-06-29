using ExpenseManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Repositories
{
    public interface IExpenseRepository : IBaseRepository<Expense>
    {
        Task<IEnumerable<Expense>> GetExpensesWithFilterAsync(Expression<Func<Expense, bool>> predicate);

        Task<Expense> GetByIdAsync(Guid id);
    }
}