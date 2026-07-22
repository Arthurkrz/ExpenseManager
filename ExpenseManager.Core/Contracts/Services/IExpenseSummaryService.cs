using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Services
{
    public interface IExpenseSummaryService
    {
        Task<ExpenseSummaryResult> GetTotalsByCurrencyAsync();

        Task<ServiceResponseGeneric<ExpenseSummaryResult>> GetTotalsByCurrencyFilterAsync(ExpenseFilter filter);
    }
}
