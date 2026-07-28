using ExpenseManager.Core.Common;
using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.Entities;
using ExpenseManager.Web.Mapping.Contracts;
using ExpenseManager.Web.Mapping.CustomMappings;
using ExpenseManager.Web.Models;
using ExpenseManager.Web.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseManager.Web.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly IExpenseSummaryService _expenseSummaryService;
        private readonly IPaginatedViewModelMapper _paginatedViewModelMapper;
        private readonly IObjectMapper _mapper;
        private readonly IExchangeService _exchangeService;

        public ExpenseController(IExpenseService expenseService, IExpenseSummaryService expenseSummaryService, IPaginatedViewModelMapper paginatedViewModelMapper, IObjectMapper mapper, IExchangeService exchangeService)
        {
            _expenseService = expenseService;
            _expenseSummaryService = expenseSummaryService;
            _paginatedViewModelMapper = paginatedViewModelMapper;
            _mapper = mapper;
            _exchangeService = exchangeService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(int pageNumber = 1, int pageSize = 10)
        {
            var paginatedExpenses = await _expenseService
                .GetPagedAsync(pageNumber, pageSize);

            var viewModel = _paginatedViewModelMapper.
                ToExpensePaginatedViewModel(paginatedExpenses);

            var rates = await _exchangeService.GetExchangeAsync();

            var pageSourceTotals = CurrencyTotalCalculator
                .GroupTotalsBySourceCurrency(viewModel.Items);

            var pageTotalsByCurrency = CurrencyTotalCalculator
                .CalculateTotalsByCurrency(pageSourceTotals, rates);

            var allSourceTotals = await _expenseSummaryService
                .GetTotalsByCurrencyAsync();

            var allTotalsByCurrency = CurrencyTotalCalculator.
                CalculateTotalsByCurrency(allSourceTotals.TotalsByCurrency, rates);

            ViewBag.PageTotalsByCurrency = pageTotalsByCurrency;
            ViewBag.AllTotalsByCurrency = allTotalsByCurrency;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> FilterAsync(ExpenseFilterViewModel filterViewModel)
        {
            if (IsFilterViewModelEmpty(filterViewModel))
                return BadRequest(new
                { 
                    errors = new List<string> 
                    { "Add at least 1 search parameter." } 
                });
            
            var filterModel = _mapper.Map<ExpenseFilterViewModel, ExpenseFilter>(
                filterViewModel, options => { options.MapViewModelToFilter(); });

            var response = await _expenseService
                .GetExpensesWithFilterPagedAsync(filterModel);

            if (!response.Success) 
                return BadRequest(new 
                { errors = response.Errors });

            var viewModel = _paginatedViewModelMapper
                .ToExpensePaginatedViewModel(response.Data);

            return PartialView("_FilteredExpenseResults", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(ExpenseViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { 
                    errors = GetModelStateErrors() });

            var entity = _mapper.Map<ExpenseViewModel, Expense>(
                model, options => options.MapViewModelToExpense());

            var result = await _expenseService.CreateExpenseAsync(entity);

            if (!result.Success)
                return BadRequest(new { errors = result.Errors });

            TempData["SuccessMessage"] = "Expense registered successfully!";

            return Ok(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(ExpenseViewModel expenseVM)
        {
            if (expenseVM.Id == Guid.Empty)
                return BadRequest(new { errors = 
                    new List<string> { "Invalid expense ID." } });

            if (!ModelState.IsValid)
                return BadRequest(new { 
                    errors = GetModelStateErrors() });

            var entity = _mapper.Map<ExpenseViewModel, Expense>(
                expenseVM, options =>
                    { options.MapViewModelToExpense(); });

            var result = await _expenseService.UpdateExpenseAsync(entity);

            if (!result.Success)
                return BadRequest(new { 
                    errors = result.Errors });

            TempData["SuccessMessage"] = "Expense updated successfully!";

            return Ok(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var result = await _expenseService.DeleteExpenseAsync(id);

            if (!result.Success)
            {
                TempData["ErrorMessage"] = string.Join("<br>", result.Errors);
                return RedirectToAction("Index");
            }

            TempData["SuccessMessage"] = "Expense deleted successfully!";

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpGet]
        public IActionResult Filter() => View(
            new ExpenseFilterViewModel 
            { 
                ValueStringRangeStart = string.Empty, 
                ValueStringRangeEnd = string.Empty, 
                Expenses = [] 
            });

        private bool IsFilterViewModelEmpty(ExpenseFilterViewModel filterViewModel) =>
            filterViewModel is null 
            || string.IsNullOrWhiteSpace(filterViewModel.NameContains)
            && string.IsNullOrWhiteSpace(filterViewModel.SourceContains)
            && string.IsNullOrWhiteSpace(filterViewModel.ValueStringRangeStart)
            && string.IsNullOrWhiteSpace(filterViewModel.ValueStringRangeEnd)
            && filterViewModel.DateRangeStart is null
            && filterViewModel.DateRangeEnd is null
            && filterViewModel.Month is null
            && filterViewModel.Currency is null
            && filterViewModel.Type is null;

        private List<string> GetModelStateErrors() =>
            ModelState.Values.SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage).ToList();
    }
}