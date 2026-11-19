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

            RuleFor(b => b.Name)
                .MaximumLength(50)
                .WithMessage("Expense name must be less than 50 characters");

            RuleFor(b => b.Currency)
                .NotNull()
                .WithMessage("Currency is required");

            RuleFor(b => b.Value)
                .NotEqual(0)
                .WithMessage("Value is required");

            RuleFor(b => b.Value)
                .GreaterThan(-1)
                .WithMessage("Value must not be negative");

            RuleFor(b => b.Value)
                .LessThan(1000000)
                .WithMessage("Value must be lower than a million");

            RuleFor(b => b.Type)
                .NotNull()
                .WithMessage("Expense category is required");

            RuleFor(b => b.ExpenseDate)
                .NotNull().NotEqual(default(DateTime))
                .WithMessage("Expense date is required");

            RuleFor(b => b.ExpenseDate)
                .GreaterThan(DateTime.Now.AddYears(-1))
                .When(b => b.ExpenseDate != default)
                .WithMessage("Expenses must be from less than 1 year ago");

            RuleFor(b => b.Source).Must(n => 
                !string.IsNullOrEmpty(n))
                .WithMessage("Expense source is required");

            RuleFor(b => b.Source)
                .MaximumLength(50)
                .WithMessage("Expense source must be less than 50 characters");
        }
    }
}
