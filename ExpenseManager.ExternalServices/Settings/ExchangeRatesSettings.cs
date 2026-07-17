using System.Collections.Generic;

namespace ExpenseManager.ExternalServices.Settings
{
    public class ExchangeRatesSettings
    {
        public string Provider { get; set; } = "fixed";
        
        public Dictionary<string, decimal> Fixed { get; set; } = [];
    }
}
