using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using System;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Services
{
    public interface IExpenseService
    {
        Task<ServiceResponse> CreateExpenseAsync(Expense entity);

        Task<ServiceResponse> UpdateExpenseAsync(Expense Expense);

        Task<ServiceResponse> DeleteExpenseAsync(Guid id);

        Task<PaginatedResult<Expense>> GetPagedAsync(int pageNumber, int pageSize);

        Task<ServiceResponseGeneric<PaginatedResult<Expense>>> GetExpensesWithFilterPagedAsync(ExpenseFilter filter);
    }
}
