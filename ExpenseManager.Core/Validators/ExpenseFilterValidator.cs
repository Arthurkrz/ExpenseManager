using ExpenseManager.Core.Entities;
using FluentValidation;
using System;

namespace ExpenseManager.Core.Validators
{
    public class ExpenseFilterValidator : AbstractValidator<ExpenseFilter>
    {
        public ExpenseFilterValidator()
        {
            RuleFor(f => f.DateRangeStart)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .WithMessage("Expenses from more than 1 year ago can't be listed");

            RuleFor(f => f.DateRangeStart)
                .LessThan(f => f.DateRangeEnd)
                .WithMessage("Start of range for expense date must be lower than end of range");

            RuleFor(f => f.ValueRangeStart)
                .LessThan(f => f.ValueRangeEnd)
                .WithMessage("Start of range for value must be lower than end of range");

            RuleFor(f => f.ValueRangeStart)
                .GreaterThan(0)
                .WithMessage("Start of range for value must be higher than zero");

            RuleFor(f => f.ValueRangeEnd)
                .LessThan(1000000)
                .WithMessage("End of range for value must be lower than a million");
        }
    }
}