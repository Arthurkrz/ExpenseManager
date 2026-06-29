using ExpenseManager.Core.Entities;
using FluentValidation;
using System;

namespace ExpenseManager.Core.Validators
{
    public class ExpenseValidator : AbstractValidator<Expense>
    {
        public ExpenseValidator()
        {
            RuleFor(b => b.Name).Must(n => 
                !string.IsNullOrEmpty(n))
               .WithMessage("Name is required");

            RuleFor(b => b.ExpenseDate)
                .NotNull().NotEqual(default(DateTime))
                .WithMessage("Expense date is required");

            RuleFor(b => b.ExpenseDate)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .WithMessage("Expenses must be from less than 1 year ago");

            RuleFor(b => b.Source).Must(n => 
                !string.IsNullOrEmpty(n))
                .WithMessage("Expense source is required");

            RuleFor(b => b.Value)
                .NotEqual(0)
                .WithMessage("Value is required");

            RuleFor(b => b.Value)
                .LessThan(1000000)
                .WithMessage("Value must be lower than a million");

            RuleFor(b => b.Currency)
                .NotNull()
                .WithMessage("Currency is required");

            RuleFor(b => b.Type)
                .NotNull()
                .WithMessage("Expense category is required");
        }
    }
}
