using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseManager.Core.Contracts.Services
{
    public interface IExchangeService
    {
        Task<Dictionary<string, decimal>> GetExchangeAsync();
    }
}
