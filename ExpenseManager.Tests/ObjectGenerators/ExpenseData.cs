using Bogus;
using ExpenseManager.Core.Entities;
using ExpenseManager.Core.Enum;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Tests.ObjectGenerators
{
    public class ExpenseData
    {
        public static IEnumerable<object[]> GetInvalidExpenses()
        {
            var faker = new Faker();

            yield return new object[]
            {
                new Expense()
                {
                    Name = null,
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Decimal(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },

                new List<string> { "Name is required" }
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = null,
                    Value = faker.Random.Decimal(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },

                new List<string> { "Currency is required" }
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = 0,
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },

                new List<string> { "Value is required" }
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = -1,
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },

                new List<string> { "Value must not be negative" }
            };

            yield return new object[]
            {   
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Decimal(1, 1000000),
                    Type = null,
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },

                new List<string> { "Expense category is required" }
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Decimal(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = default,
                    Source = faker.Random.Word()
                },

                new List<string> { "Expense date is required" }
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Decimal(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = null
                },

                new List<string> { "Expense source is required" }
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = 1000001,
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },

                new List<string> { "Value must be lower than a million" }
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Decimal(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = DateTime.Now.AddYears(-1),
                    Source = faker.Random.Word()
                },

                new List<string> { "Expenses must be from less than 1 year ago" }
            };


            yield return new object[]
            {
                new Expense()
                {
                    Name = null,
                    Currency = null,
                    Value = 0,
                    Type = null,
                    ExpenseDate = DateTime.Now.AddYears(-1),
                    Source = null
                },

                new List<string>
                {
                    "Name is required",
                    "Currency is required",
                    "Value is required",
                    "Expense category is required",
                    "Expense source is required",
                    "Expenses must be from less than 1 year ago"
                }
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = "",
                    Currency = null,
                    Value = -1,
                    Type = null,
                    ExpenseDate = default,
                    Source = ""
                },

                new List<string>
                {
                    "Name is required",
                    "Currency is required",
                    "Value must not be negative",
                    "Expense category is required",
                    "Expense date is required",
                    "Expense source is required"
                }
            };
        }
    }
}
