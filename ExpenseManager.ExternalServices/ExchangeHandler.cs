using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.DTOs;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExpenseManager.ExternalServices
{
    public class ExchangeHandler : IExchangeHandler
    {
        public async Task<ExchangeResultDTO> GetExchangeOfDayAsync()
        {
            ExchangeResultDTO result = new();

            using (var client = new HttpClient())
            {
                string token = "dbcd1ba81b22497fc218af43f6658209";
                string _base = "EUR";
                string symbols = "BRL";
                var url = new Uri($"https://api.exchangeratesapi.io/v1/latest?access_key={token}&base={_base}&symbols={symbols}");

                var response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var options = new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true,
                        WriteIndented = true
                    };

                    var content = await response.Content.ReadAsStringAsync();

                    return JsonSerializer.Deserialize<ExchangeResultDTO>(content, options);
                }
            }

            return result;
        }
    }
}