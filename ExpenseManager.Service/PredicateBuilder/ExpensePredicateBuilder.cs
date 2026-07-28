using ExpenseManager.Core.Entities;
using System;
using System.Linq.Expressions;

namespace ExpenseManager.Service.PredicateBuilder
{
    public static class ExpensePredicateBuilder
    {
        public static Expression<Func<Expense, bool>> Build(ExpenseFilter filter)
        {
            var dateRangeStart = filter.DateRangeStart
                ?? DateTime.Today.AddYears(-1);

            var dateRangeEnd = filter.DateRangeEnd.HasValue
                ? filter.DateRangeEnd.Value.Date.AddDays(1).AddTicks(-1)
                : DateTime.Today.AddDays(1).AddTicks(-1);

            Expression<Func<Expense, bool>> predicate = expense => true;

            if (!string.IsNullOrEmpty(filter.NameContains))
                predicate = predicate.AndAlso(e => e.Name != null
                    && e.Name.ToLower().Contains(filter
                    .NameContains.Trim().ToLower()));

            if (!string.IsNullOrEmpty(filter.SourceContains))
                predicate = predicate.AndAlso(e => e.Source != null
                    && e.Source.ToLower().Contains(filter
                    .SourceContains.Trim().ToLower()));

            if (filter.ValueRangeStart.HasValue)
                predicate = predicate.AndAlso(e => e.Value >=
                    filter.ValueRangeStart.Value);

            if (filter.ValueRangeEnd.HasValue)
                predicate = predicate.AndAlso(e => e.Value <=
                    filter.ValueRangeEnd.Value);

            predicate = predicate.AndAlso(e =>
                e.ExpenseDate >= dateRangeStart &&
                e.ExpenseDate <= dateRangeEnd);

            if (filter.Currency.HasValue)
                predicate = predicate.AndAlso(e => 
                    e.Currency == filter.Currency.Value);

            if (filter.Type.HasValue)
                predicate = predicate.AndAlso(e => 
                    e.Type == filter.Type.Value);

            if (filter.Month.HasValue)
            {
                var filterDateStart = new DateTime(
                    DateTime.Now.Year, (int)filter.Month.Value, 1);

                var filterDateEnd = filterDateStart
                    .AddMonths(1).AddTicks(-1);

                predicate = predicate.AndAlso(e => 
                    e.ExpenseDate >= filterDateStart && 
                    e.ExpenseDate <= filterDateEnd);
            }

            return predicate;
        }

        private static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            var parameter = left.Parameters[0];

            var rightBody = new ReplaceParameterVisitor(
                right.Parameters[0], parameter)
                    .Visit(right.Body);

            var body = Expression.AndAlso(left.Body, rightBody!);

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }
    }
}
