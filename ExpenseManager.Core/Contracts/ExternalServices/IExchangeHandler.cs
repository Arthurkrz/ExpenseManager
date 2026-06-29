using ExpenseManager.Core.DTOs;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.ExternalServices
{
    public interface IExchangeHandler
    {
        Task<ExchangeResultDTO> GetExchangeOfDayAsync();
    }
}
