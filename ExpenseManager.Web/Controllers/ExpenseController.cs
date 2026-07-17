using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.Entities;
using ExpenseManager.Web.Mapping.Contracts;
using ExpenseManager.Web.Mapping.CustomMappings;
using ExpenseManager.Web.Models;
using ExpenseManager.Web.Models.Enum;
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
        private readonly IObjectMapper _mapper;
        private readonly IExchangeService _exchangeService;

        public ExpenseController(IExpenseService expenseService, IObjectMapper mapper, IExchangeService exchangeService)
        {
            _expenseService = expenseService;
            _mapper = mapper;
            _exchangeService = exchangeService;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var expenses = await _expenseService.GetAllAsync();

            var expensesVM = _mapper.MapCollection<Expense, ExpenseViewModel>(
                expenses, options => options.MapExpenseToViewModel());

            var rates = await _exchangeService.GetExchangeAsync();

            var totalsByCurrency = CalculateTotalsByCurrency(expensesVM, rates);

            ViewBag.TotalsByCurrency = totalsByCurrency;

            return View(expensesVM);
        }

        [HttpGet]
        public IActionResult FilterAsync()
        {
            var filterViewModel = new ExpenseFilterViewModel
            {
                ValueStringRangeStart = string.Empty,
                ValueStringRangeEnd = string.Empty,
                Expenses = []
            };

            return View(filterViewModel);
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
        public async Task<IActionResult> FilterAsync(ExpenseFilterViewModel filterViewModel)
        {
            if (IsFilterViewModelEmpty(filterViewModel))
                return BadRequest(new { errors = new List<string> 
                    { "Add at least 1 search parameter." } });

            var filterModel = _mapper.Map<ExpenseFilterViewModel, ExpenseFilter>(
                filterViewModel, options => { options.MapViewModelToFilter(); });

            var response = await _expenseService.GetExpensesWithFilterAsync(filterModel);

            if (!response.Success) return BadRequest(
                new { errors = response.Errors });

            IEnumerable<Expense> expenses = response.Data ?? [];

            var expensesViewModel = _mapper.MapCollection<Expense, ExpenseViewModel>(
                expenses, options => options.MapExpenseToViewModel());

            return PartialView("_ExpenseTable", expensesViewModel);
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

        private Dictionary<string, decimal> CalculateTotalsByCurrency(IEnumerable<ExpenseViewModel> expenses, Dictionary<string, decimal> rates)
        {
            EnsureBaseRate(rates);

            var totals = new Dictionary<string, decimal>();

            foreach (var targetCurrency in Enum.GetNames<CurrencyVM>())
            {
                decimal total = expenses.Sum(expense =>
                    ConvertCurrency(expense.Value,
                        expense.Currency?.ToString(),
                        targetCurrency, rates));

                totals[targetCurrency] = total;
            }

            return totals;
        }

        private decimal ConvertCurrency(decimal amount, string sourceCurrency, string targetCurrency, Dictionary<string, decimal> rates) =>
            string.IsNullOrWhiteSpace(sourceCurrency)
            || sourceCurrency == targetCurrency
            || !rates.TryGetValue(sourceCurrency, out var sourceRate)
            || !rates.TryGetValue(targetCurrency, out var targetRate)
            || sourceRate == 0
                ? amount
                : amount / sourceRate * targetRate;

        private void EnsureBaseRate(Dictionary<string, decimal> rates) =>
            rates.TryAdd("USD", 1.0m);

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