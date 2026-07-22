using ExpenseManager.Core.Entities;
using FluentValidation;

namespace ExpenseManager.Core.Validators
{
    public class ExpenseFilterValidator : AbstractValidator<ExpenseFilter>
    {
        public ExpenseFilterValidator()
        {
            RuleFor(f => f.ValueRangeStart)
                .LessThan(f => f.ValueRangeEnd)
                .WithMessage("Start of range for value must be lower than end of range");

            RuleFor(f => f.ValueRangeStart)
                .GreaterThan(0)
                .WithMessage("Start of range for value must be higher than zero");

            RuleFor(f => f.ValueRangeEnd)
                .LessThan(1000000)
                .WithMessage("End of range for value must be lower than a million");

            RuleFor(f => f.DateRangeStart)
                .LessThan(f => f.DateRangeEnd)
                .WithMessage("Start of range for expense date must be lower than end of range");
        }
    }
}