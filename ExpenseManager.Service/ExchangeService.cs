using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.Contracts.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseManager.Service
{
    public class ExchangeService : IExchangeService
    {
        private readonly IExchangeHandler _exchangeHandler;
        private readonly IMemoryCacheService _memoryCacheService;

        public ExchangeService(IExchangeHandler exchangeHandler, IMemoryCacheService memoryCacheService)
        {
            _exchangeHandler = exchangeHandler;
            _memoryCacheService = memoryCacheService;
        }

        public async Task<Dictionary<string, double>> GetExchangeAsync() => 
            (await _memoryCacheService.GetOrCreateAsync("exchange", 
                _exchangeHandler.GetExchangeOfDayAsync)).Rates;
    }
}
