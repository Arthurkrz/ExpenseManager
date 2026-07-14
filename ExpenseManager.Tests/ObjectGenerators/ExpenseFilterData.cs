using ExpenseManager.Core.Entities;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Tests.ObjectGenerators
{
    public class ExpenseFilterData
    {
        public static IEnumerable<object[]> GetInvalidFilters()
        {
            yield return new object[]
            {
                new ExpenseFilter() { DateRangeStart = DateTime.Now.AddYears(-1) },
                "Expenses from more than 1 year ago can't be listed"
            };

            yield return new object[]
            {
                new ExpenseFilter() { DateRangeStart = DateTime.Now, DateRangeEnd = DateTime.Now.AddDays(-1) },
                "Start of range for expense date must be lower than end of range"
            };

            yield return new object[]
            {
                new ExpenseFilter() { ValueRangeStart = 2, ValueRangeEnd = 1 },
                "Start of range for value must be lower than end of range"
            };

            yield return new object[]
            {
                new ExpenseFilter() { ValueRangeStart = -1 },
                "Start of range for value must be higher than zero"
            };

            yield return new object[]
            {
                new ExpenseFilter() { ValueRangeEnd = 1000001 },
                "End of range for value must be lower than a million"
            };
        }
    }
}
