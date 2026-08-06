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
                new ExpenseFilter() { NameContains = "This name is way too long and exceeds the maximum length of fifty characters" },

                new List<string> { "Expense name must be less than 50 characters" }
            };

            yield return new object[]
            {
                new ExpenseFilter() { SourceContains = "This source is way too long and exceeds the maximum length of fifty characters" },

                new List<string> { "Expense source must be less than 50 characters" }
            };

            yield return new object[]
            {
                new ExpenseFilter() { DateRangeStart = DateTime.Now, DateRangeEnd = DateTime.Now.AddDays(-1) },

                new List<string> { "Start of range for expense date must be lower than end of range" }
            };

            yield return new object[]
            {
                new ExpenseFilter() { ValueRangeStart = 2, ValueRangeEnd = 1 },

                new List<string> { "Start of range for value must be lower than end of range" }
            };

            yield return new object[]
            {
                new ExpenseFilter() { ValueRangeStart = -1 },

                new List<string> { "Start of range for value must be higher than zero" }
            };

            yield return new object[]
            {
                new ExpenseFilter() { ValueRangeEnd = 1000001 },

                new List<string> { "End of range for value must be lower than a million" }
            };

            yield return new object[]
            {
                new ExpenseFilter
                {
                    NameContains = "This name is way too long and exceeds the maximum length of fifty characters",
                    SourceContains = "This source is way too long and exceeds the maximum length of fifty characters",
                    DateRangeStart = DateTime.Now,
                    DateRangeEnd = DateTime.Now.AddDays(-1),
                    ValueRangeStart = 2,
                    ValueRangeEnd = 1,

                },

                new List<string>
                {
                    "Expense name must be less than 50 characters",
                    "Expense source must be less than 50 characters",
                    "Start of range for expense date must be lower than end of range",
                    "Start of range for value must be lower than end of range"
                }
            };

            yield return new object[]
            {
                new ExpenseFilter
                {
                    NameContains = "This name is way too long and exceeds the maximum length of fifty characters",
                    SourceContains = "This source is way too long and exceeds the maximum length of fifty characters",
                    DateRangeStart = DateTime.Now,
                    DateRangeEnd = DateTime.Now.AddDays(-1),
                    ValueRangeStart = -1,
                    ValueRangeEnd = 1000001,

                },

                new List<string>
                {
                    "Expense name must be less than 50 characters",
                    "Expense source must be less than 50 characters",
                    "Start of range for expense date must be lower than end of range",
                    "Start of range for value must be higher than zero",
                    "End of range for value must be lower than a million"
                }
            };
        }
    }
}
