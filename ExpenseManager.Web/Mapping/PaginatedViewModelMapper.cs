using ExpenseManager.Core.Common;
using ExpenseManager.Core.Entities;
using ExpenseManager.Web.Mapping.Contracts;
using ExpenseManager.Web.Mapping.CustomMappings;
using ExpenseManager.Web.Models;
using System;
using System.Linq;

namespace ExpenseManager.Web.Mapping
{
    public class PaginatedViewModelMapper : IPaginatedViewModelMapper
    {
        private readonly IObjectMapper _mapper;

        public PaginatedViewModelMapper(IObjectMapper mapper)
        {
            ArgumentNullException.ThrowIfNull(mapper, nameof(mapper));

            _mapper = mapper;
        }

        public PaginatedViewModel<ExpenseViewModel> ToExpensePaginatedViewModel(PaginatedResult<Expense> paginatedExpenses)
        {
            ArgumentNullException.ThrowIfNull(paginatedExpenses, nameof(paginatedExpenses));

            var expensesViewModel = _mapper.MapCollection<Expense, ExpenseViewModel>(
                paginatedExpenses.Items, options => options.MapExpenseToViewModel());

            return new PaginatedViewModel<ExpenseViewModel>
            {
                Items = expensesViewModel.ToList(),
                PageNumber = paginatedExpenses.PageNumber,
                PageSize = paginatedExpenses.PageSize,
                TotalCount = paginatedExpenses.TotalCount
            };
        }
    }
}
