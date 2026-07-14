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

                "Name is required"
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

                "Currency is required"
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

                "Value is required"
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

                "Expense category is required"
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

                "Expense date is required"
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

                "Expense source is required"
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

                "Value must be lower than a million"
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

                "Expenses must be from less than 1 year ago"
            };
        }
    }
}
