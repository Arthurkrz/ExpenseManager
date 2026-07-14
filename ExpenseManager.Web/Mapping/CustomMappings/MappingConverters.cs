using System.Globalization;

namespace ExpenseManager.Web.Mapping.CustomMappings
{
    public static class MappingConverters
    {
        public static decimal? StringToMoney(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0.0M;

            value = value.Replace(',', '.');

            return decimal.TryParse(value, NumberStyles.Any, 
                CultureInfo.InvariantCulture, out var result) 
                    ? result : 0;
        }

        public static string MoneyToString(decimal value) =>
            value.ToString("F2", CultureInfo.InvariantCulture);
    }
}
