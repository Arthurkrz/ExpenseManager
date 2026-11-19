using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.DTOs;
using ExpenseManager.ExternalServices.Settings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseManager.ExternalServices
{
    public class ApiExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeApiSettings _settings;

        public ApiExchangeRateProvider(HttpClient httpClient, IOptions<ExchangeApiSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<ExchangeResultDTO> GetExchangeOfDayAsync()
        {
            if (string.IsNullOrWhiteSpace(_settings.AccessKey))
                return new ExchangeResultDTO();

            var symbols = string.Join(",", _settings.Symbols);

            var url = 
                $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.LatestEndpoint}" +
                $"?access_key={Uri.EscapeDataString(_settings.AccessKey)}" +
                $"&symbols={Uri.EscapeDataString(symbols)}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new ExchangeResultDTO();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions 
                { PropertyNameCaseInsensitive = true };

            var result = JsonSerializer.Deserialize<ExchangeResultDTO>(content, options)
                ?? new ExchangeResultDTO();

            return ConvertToUSD(result);
        }

        private ExchangeResultDTO ConvertToUSD(ExchangeResultDTO euroBaseResult)
        {
            if (euroBaseResult?.Rates == null 
                || !euroBaseResult.Rates.TryGetValue("USD", out var usdPerEuro) 
                || usdPerEuro == 0)
                return euroBaseResult;

            var usdCenteredRates = new Dictionary<string, decimal>();

            foreach (var rate in euroBaseResult.Rates)
            {
                if (rate.Key == "USD") usdCenteredRates["USD"] = 1m;
                else usdCenteredRates[rate.Key] = rate.Value / usdPerEuro;
            }

            usdCenteredRates["EUR"] = 1m / usdPerEuro;

            return new ExchangeResultDTO
            {
                Date = euroBaseResult.Date,
                Rates = usdCenteredRates
            };
        }
    }
}