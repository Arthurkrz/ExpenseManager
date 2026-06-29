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
                new ExpenseFilter()
                { DateRangeStart = DateTime.Now.AddYears(-1) },
                @"Não é possível listar despesas de mais de 1 ano atrás."
            };

            yield return new object[]
            {
                new ExpenseFilter()
                {
                    DateRangeStart = DateTime.Now,
                    DateRangeEnd = DateTime.Now.AddDays(-1)
                },
                @"O intervalo inicial de data da despesa não 
                  pode ser maior que o intervalo final."
            };

            yield return new object[]
            {
                new ExpenseFilter()
                {
                    ValueRangeStart = 2,
                    ValueRangeEnd = 1
                },
                @"O intervalo inicial de valor da despesa não 
                  pode ser maior que o intervalo final."
            };

            yield return new object[]
            {
                new ExpenseFilter()
                { ValueRangeStart = -1 },
                @"O intervalo inicial de valor da despesa não 
                  pode ser menor que zero."
            };

            yield return new object[]
            {
                new ExpenseFilter()
                { ValueRangeEnd = 1000001 },
                @"O intervalo final de valor da despesa não 
                  pode ser maior que 1 milhão."
            };
        }
    }
}
