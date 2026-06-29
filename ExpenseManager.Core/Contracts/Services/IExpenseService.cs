using ExpenseManager.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Services
{
    public interface IExpenseService
    {
        Task<ServiceResponse> CreateExpenseAsync(Expense entity);

        Task<ServiceResponse> UpdateExpenseAsync(Expense Expense);

        Task<ServiceResponse> DeleteExpenseAsync(Guid id);

        Task<ServiceResponseGeneric<IEnumerable<Expense>>> GetExpensesWithFilterAsync(ExpenseFilter filter);

        Task<IEnumerable<Expense>> GetAllAsync();
    }
}
