using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.DTOs;
using ExpenseManager.ExternalServices.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace ExpenseManager.ExternalServices
{
    public class FixedExchangeRateProvider : IExchangeRateProvider
    {
        private readonly ExchangeRatesSettings _settings;

        public FixedExchangeRateProvider(IOptions<ExchangeRatesSettings> settings)
        {
            _settings = settings.Value;
        }

        public Task<ExchangeResultDTO> GetExchangeOfDayAsync()
        {
            var result = new ExchangeResultDTO
            {
                Date = DateTime.UtcNow.Date,
                Base = _settings.BaseCurrency,
                Rates = _settings.Fixed
            };

            return Task.FromResult(result);
        }
    }
}
