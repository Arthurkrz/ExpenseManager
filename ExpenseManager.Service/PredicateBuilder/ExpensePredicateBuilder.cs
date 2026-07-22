using ExpenseManager.Core.Entities;
using LinqKit;
using System;
using System.Linq.Expressions;

namespace ExpenseManager.Service.PredicateBuilder
{
    public static class ExpensePredicateBuilder
    {
        public static Expression<Func<Expense, bool>> Build(ExpenseFilter filter)
        {
            var dateRangeStart = filter.DateRangeStart ?? DateTime.Now.AddYears(-1);
            var dateRangeEnd = filter.DateRangeEnd ?? DateTime.Now;

            Expression<Func<Expense, bool>> predicate = b => true;

            if (!string.IsNullOrEmpty(filter.NameContains))
                predicate = predicate.And(b => b.Name
                    .ToLower().Contains(filter.NameContains
                        .Trim().ToLower()));

            if (!string.IsNullOrEmpty(filter.SourceContains))
                predicate = predicate.And(b => b.Source
                    .ToLower().Contains(filter.SourceContains
                        .Trim().ToLower()));

            if (filter.ValueRangeStart.HasValue)
                predicate = predicate.And(b => b.Value >= 
                    filter.ValueRangeStart.Value);

            if (filter.ValueRangeEnd.HasValue)
                predicate = predicate.And(b => b.Value <= 
                    filter.ValueRangeEnd.Value);

            predicate = predicate.And(b =>
                b.ExpenseDate >= dateRangeStart &&
                b.ExpenseDate <= dateRangeEnd);

            if (filter.Currency.HasValue)
                predicate = predicate.And(b => b.Currency == 
                    filter.Currency.Value);

            if (filter.Type.HasValue)
                predicate = predicate.And(b => b.Type == 
                    filter.Type.Value);

            if (filter.Month.HasValue)
            {
                var filterDateStart = new DateTime(
                    DateTime.Now.Year, (int)filter.Month.Value, 1);

                var filterDateEnd = filterDateStart
                    .AddMonths(1).AddDays(-1);

                predicate = predicate.And(
                    b => b.ExpenseDate >= filterDateStart 
                    && b.ExpenseDate <= filterDateEnd);
            }

            return predicate;
        }
    }
}
