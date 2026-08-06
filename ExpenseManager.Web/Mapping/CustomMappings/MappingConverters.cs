using System;
using System.Globalization;
using System.Linq;

namespace ExpenseManager.Web.Mapping.CustomMappings
{
    public static class MappingConverters
    {
        private const NumberStyles STYLES =
            NumberStyles.AllowLeadingSign | 
            NumberStyles.AllowDecimalPoint;

        public static decimal StringToMoney(string value)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value);

            if (!TryParseMoney(value, out var result))
                    throw new FormatException($"The value" +
                        $" '{value}' is not a valid " +
                        $"monetary value.");

            return result;
        }

        public static decimal? StringToNullableMoney(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            if (!TryParseMoney(value, out var result))
                throw new FormatException($"The value" +
                    $" '{value}' is not a valid " +
                    $"monetary value.");

            return result;
        }

        public static string MoneyToString(decimal value) =>
            value.ToString("F2", CultureInfo.InvariantCulture);

        private static bool TryParseMoney(string value, out decimal result)
        {
            var normalizedValue = value.Trim();

            var commaCount = normalizedValue.Count(character => character == ',');
            var dotCount = normalizedValue.Count(character => character == '.');

            if (commaCount > 1 || dotCount > 1)
            {
                result = default;
                return false;
            }

            normalizedValue = normalizedValue.Replace(',', '.');

            return decimal.TryParse(normalizedValue, STYLES, 
                CultureInfo.InvariantCulture, out result);
        }
    }
}
