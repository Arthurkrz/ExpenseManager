using ExpenseManager.Core.Contracts.Repositories;
using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.Entities;
using ExpenseManager.Service.PredicateBuilder;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ExpenseManager.Service
{
    public class ExpenseService : IExpenseService
    {
        private readonly IValidator<Expense> _validatorExpense;
        private readonly IValidator<ExpenseFilter> _validatorExpenseFilter;
        private readonly IExpenseRepository _expenseRepository;   

        public ExpenseService(IExpenseRepository expenseRepository, 
                           IValidator<Expense> validatorExpense, 
                           IValidator<ExpenseFilter> validatorExpenseFilter)
        {
            _expenseRepository = expenseRepository;
            _validatorExpense = validatorExpense;
            _validatorExpenseFilter = validatorExpenseFilter;
        }

        public async Task<ServiceResponse> CreateExpenseAsync(Expense expense)
        {
            if (expense is null) return new ServiceResponse 
                { Success = false, Errors = ["Expense must not be null"] };

            var result = _validatorExpense.Validate(expense);

            if (!result.IsValid) 
                return new ServiceResponse
                { Success = false, Errors = result
                    .Errors.Select(e => e.ErrorMessage).ToList() };

            await _expenseRepository.AddAsync(expense);

            return new ServiceResponse { Success = true };
        }

        public async Task<IEnumerable<Expense>> GetAllAsync() =>
            await _expenseRepository.GetAllAsync();

        public async Task<ServiceResponseGeneric<IEnumerable<Expense>>> GetExpensesWithFilterAsync(ExpenseFilter filter)
        {
            var validationResult = _validatorExpenseFilter.Validate(filter);

            if (!validationResult.IsValid)
                return new ServiceResponseGeneric<IEnumerable<Expense>>
                { Success = false, Errors = validationResult
                    .Errors.Select(e => e.ErrorMessage).ToList() };

            Expression<Func<Expense, bool>> filterExpression = 
                ExpensePredicateBuilder.Build(filter);

            var expenses = await _expenseRepository.
                GetExpensesWithFilterAsync(filterExpression);

            return new ServiceResponseGeneric<IEnumerable<Expense>>
                { Success = true, Data = expenses };
        }

        public async Task<ServiceResponse> UpdateExpenseAsync(Expense expense)
        {
            var existingExpense = await _expenseRepository.GetByIdAsync(expense.Id);

            if (existingExpense is null) return new ServiceResponse
                { Success = false, Errors = ["Expense not found."] };

            var result = _validatorExpense.Validate(expense);

            if (!result.IsValid)
                return new ServiceResponse
                { Success = false, Errors = result
                    .Errors.Select(e => e.ErrorMessage).ToList() };

            await _expenseRepository.UpdateAsync(expense);

            return new ServiceResponse { Success = true };
        }

        public async Task<ServiceResponse> DeleteExpenseAsync(Guid id)
        {
            var entity = await _expenseRepository.GetByIdAsync(id);

            if (entity is null) return new ServiceResponse
                { Success = false, Errors = ["ID does not match any expense."] };

            await _expenseRepository.DeleteAsync(entity);

            return new ServiceResponse { Success = true };
        }
    }
}