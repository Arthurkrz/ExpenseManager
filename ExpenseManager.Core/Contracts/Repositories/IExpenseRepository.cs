using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Core.Contracts.Repositories
{
    public interface IExpenseRepository : IBaseRepository<Expense> 
    {
        Task<PaginatedResult<Expense>> GetExpensesPagedAsync(int pageNumber, int pageSize);
        
        Task<PaginatedResult<Expense>> GetExpensesWithFilterPagedAsync(Expression<Func<Expense, bool>> predicate, int pageNumber, int pageSize);
    }
}