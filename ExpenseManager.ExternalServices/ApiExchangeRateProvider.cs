using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.DTOs;
using ExpenseManager.ExternalServices.Settings;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseManager.ExternalServices
{
    public class ApiExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ExchangeApiSettings _settings;

        public ApiExchangeRateProvider(HttpClient httpClient, ExchangeApiSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<ExchangeResultDTO> GetExchangeOfDayAsync()
        {
            if (string.IsNullOrWhiteSpace(_settings.AccessKey))
                return new ExchangeResultDTO();

            var symbols = string.Join(",", _settings.Symbols);

            var url = 
                $"{_settings.BaseUrl.TrimEnd('/')}/{_settings.LatestEndpoint}" +
                $"?access_key={Uri.EscapeDataString(_settings.AccessKey)}" +
                $"&base={Uri.EscapeDataString(_settings.BaseCurrency)}" +
                $"&symbols={Uri.EscapeDataString(symbols)}";

            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return new ExchangeResultDTO();

            var content = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions 
                { PropertyNameCaseInsensitive = true };

            return JsonSerializer.Deserialize<ExchangeResultDTO>(content, options)
                ?? new ExchangeResultDTO();
        }
    }
}