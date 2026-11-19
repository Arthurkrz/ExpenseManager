using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ExpenseManager.Core.DTOs
{
    public class ExchangeResultDTO
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }
        
        [JsonPropertyName("rates")]
        public Dictionary<string, decimal> Rates { get; set; }
    }
}
