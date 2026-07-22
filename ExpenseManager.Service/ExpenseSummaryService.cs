using ExpenseManager.Core.Common;
using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.Entities;
using ExpenseManager.Service.PredicateBuilder;
using FluentValidation;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Service
{
    public class ExpenseSummaryService : IExpenseSummaryService
    {
        private readonly IExpenseSummaryRepository _expenseSummaryRepository;
        private readonly IValidator<ExpenseFilter> _expenseFilterValidator;

        public ExpenseSummaryService(IExpenseSummaryRepository expenseSummaryRepository, IValidator<ExpenseFilter> expenseFilterValidator)
        {
            _expenseSummaryRepository = expenseSummaryRepository;
            _expenseFilterValidator = expenseFilterValidator;
        }

        public async Task<ExpenseSummaryResult> GetTotalsByCurrencyAsync() =>
            await _expenseSummaryRepository.GetTotalsByCurrencyAsync();

        public async Task<ServiceResponseGeneric<ExpenseSummaryResult>> GetTotalsByCurrencyFilterAsync(ExpenseFilter filter)
        {
            var validationResult = _expenseFilterValidator.Validate(filter);

            if (!validationResult.IsValid)
                return new ServiceResponseGeneric<ExpenseSummaryResult>
                {
                    Success = false,
                    Errors = validationResult.Errors
                        .Select(err => err.ErrorMessage)
                        .ToList()
                };

            var filterExpression = ExpensePredicateBuilder.Build(filter);

            var totals = await _expenseSummaryRepository
                .GetTotalsByCurrencyFilterAsync(filterExpression);

            return new ServiceResponseGeneric<ExpenseSummaryResult>
            {
                Success = true,
                Data = totals
            };
        }
    }
}
