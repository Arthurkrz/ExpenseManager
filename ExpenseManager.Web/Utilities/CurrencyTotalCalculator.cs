using ExpenseManager.Web.Models.Enum;
using ExpenseManager.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpenseManager.Web.Utilities
{
    public static class CurrencyTotalCalculator
    {
        public static Dictionary<string, decimal> CalculateTotalsByCurrency(Dictionary<string, decimal> sourceTotals, Dictionary<string, decimal> rates)
        {
            rates ??= [];
            sourceTotals ??= [];

            EnsureBaseRate(rates);

            var totals = new Dictionary<string, decimal>();

            foreach (var targetCurrency in Enum.GetNames<CurrencyVM>())
            {
                var total = sourceTotals.Sum(st =>
                    ConvertCurrency(st.Value, st.Key,
                        targetCurrency, rates));

                totals[targetCurrency] = total;
            }

            return totals;
        }

        public static Dictionary<string, decimal> GroupTotalsBySourceCurrency(IEnumerable<ExpenseViewModel> expenses) =>
            expenses.Where(e => e.Currency.HasValue)
                .GroupBy(e => e.Currency.Value.ToString())
                .ToDictionary
                (
                    group => group.Key,
                    group => group.Sum(e => e.Value)
                ) ?? [];

        private static decimal ConvertCurrency(decimal amount, string sourceCurrency, string targetCurrency, Dictionary<string, decimal> rates) =>
        string.IsNullOrWhiteSpace(sourceCurrency)
        || sourceCurrency == targetCurrency
        || !rates.TryGetValue(sourceCurrency, out var sourceRate)
        || !rates.TryGetValue(targetCurrency, out var targetRate)
        || sourceRate == 0
            ? amount
            : amount / sourceRate * targetRate;

        private static void EnsureBaseRate(Dictionary<string, decimal> rates) =>
            rates.TryAdd("USD", 1.0m);
    }
}
