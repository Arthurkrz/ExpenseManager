using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using ExpenseManager.Web.Models;

namespace ExpenseManager.Web.Mapping.Contracts
{
    public interface IPaginatedViewModelMapper
    {
        PaginatedViewModel<ExpenseViewModel> ToExpensePaginatedViewModel(PaginatedResult<Expense> paginatedExpenses);
    }
}
