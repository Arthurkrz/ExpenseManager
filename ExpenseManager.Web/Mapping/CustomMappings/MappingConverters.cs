using System.Globalization;

namespace ExpenseManager.Web.Mapping.CustomMappings
{
    public static class MappingConverters
    {
        public static decimal StringToMoney(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return 0.0m;

            value = value.Replace(',', '.');

            return decimal.TryParse(value, NumberStyles.Any, 
                CultureInfo.InvariantCulture, out var result) 
                    ? result : 0m;
        }

        public static decimal? StringToNullableMoney(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            value = value.Replace(',', '.');

            return decimal.TryParse(value, NumberStyles.Any,
                CultureInfo.InvariantCulture, out var result)
                    ? result : null;
        }

        public static string MoneyToString(decimal value) =>
            value.ToString("F2", CultureInfo.InvariantCulture);
    }
}
