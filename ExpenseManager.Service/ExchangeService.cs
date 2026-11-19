using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.Contracts.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExpenseManager.Service
{
    public class ExchangeService : IExchangeService
    {
        private const string EXCHANGECACHEKEY = "exchange";
        
        private readonly IExchangeRateProvider _exchangeProvider;
        private readonly ICacheService _cacheService;

        public ExchangeService(IExchangeRateProvider exchangeProvider, ICacheService cacheService)
        {
            _exchangeProvider = exchangeProvider;
            _cacheService = cacheService;
        }

        public async Task<Dictionary<string, decimal>> GetExchangeAsync() =>
            (await _cacheService.GetOrCreateAsync(EXCHANGECACHEKEY, 
                _exchangeProvider.GetExchangeOfDayAsync)).Rates ??
                    new Dictionary<string, decimal>();
    }
}
