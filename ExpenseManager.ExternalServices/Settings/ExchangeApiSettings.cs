using System.Collections.Generic;

namespace ExpenseManager.ExternalServices.Settings
{
    public class ExchangeApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        
        public string LatestEndpoint { get; set; } = "latest";
        
        public string AccessKey { get; set; } = string.Empty;
        
        public string BaseCurrency { get; set; } = "USD";
        
        public List<string> Symbols { get; set; } = [];
    }
}
