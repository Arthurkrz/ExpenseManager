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
                    Value = faker.Random.Double(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },
                "Insira um nome."
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = null,
                    Value = faker.Random.Double(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },
                "Especifique a moeda."
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
                "Especifique o valor da despesa."
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Double(1, 1000000),
                    Type = null,
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = faker.Random.Word()
                },
                "Especifique a categoria da despesa."
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Double(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = default,
                    Source = faker.Random.Word()
                },
                "Insira a data da despesa."
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Double(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = faker.Date.Future(1, DateTime.Now),
                    Source = null
                },
                "Insira a origem da despesa."
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
                @"O valor da despesa não pode ser maior que R$ 1 milhão."
            };

            yield return new object[]
            {
                new Expense()
                {
                    Name = faker.Name.FirstName(),
                    Currency = faker.PickRandom<Currency>(),
                    Value = faker.Random.Double(1, 1000000),
                    Type = faker.PickRandom<ExpenseType>(),
                    ExpenseDate = DateTime.Now.AddYears(-1),
                    Source = faker.Random.Word()
                },
                @"Despesas de mais de 1 ano atrás não podem ser adicionadas."
            };
        }
    }
}
