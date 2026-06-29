using ExpenseManager.Core.Contracts.ExternalServices;
using ExpenseManager.Core.Contracts.Mapping;
using ExpenseManager.Core.Contracts.Services;
using ExpenseManager.Core.Entities;
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
        private readonly IMap _mapper;
        private readonly IExchangeService _exchangeService;

        public ExpenseController(IExpenseService expenseService, IMap mapper, IExchangeService exchangeService)
        {
            _expenseService = expenseService;
            _mapper = mapper;
            _exchangeService = exchangeService;
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var expenses = await _expenseService.GetAllAsync();
            var expensesVM = _mapper.Map<IEnumerable<Expense>, List<ExpenseViewModel>>(expenses);

            ViewBag.Rates = await _exchangeService.GetExchangeAsync();

            double euroToRealRate = 5.50;
            double realToEuroRate = 1 / euroToRealRate;

            double totalInReais = expensesVM.Sum(bill => bill.Currency ==
            CurrencyVM.Euro ? bill.Value * euroToRealRate : bill.Value);

            double totalInEuros = expensesVM.Sum(bill => bill.Currency ==
            CurrencyVM.Real ? bill.Value * realToEuroRate : bill.Value);

            ViewBag.TotalInReais = totalInReais;
            ViewBag.TotalInEuros = totalInEuros;

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
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                return Json(new { success = false, errors });
            }

            var entity = _mapper.Map<ExpenseViewModel, Expense>(model);

            var result = await _expenseService.CreateExpenseAsync(entity);

            if (!result.Success)
                return Json(new { success = false, errors = result.Errors });

            TempData["SuccessMessage"] = "Expense registered successfully!";

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> FilterAsync(ExpenseFilterViewModel filterViewModel)
        {
            if (IsFilterViewModelEmpty(filterViewModel))
            {
                return Json(new
                {
                    success = false,
                    errors = new List<string> { "Add at least 1 search parameter." }
                });
            }

            var filterModel = _mapper.Map<ExpenseFilterViewModel, ExpenseFilter>(filterViewModel);
            var response = await _expenseService.GetExpensesWithFilterAsync(filterModel);

            IEnumerable<Expense> expenses = response.Data;

            if (!response.Success)
                return Json(new { success = false, errors = response.Errors });

            var expensesViewModel = _mapper.Map<IEnumerable<Expense>, List<ExpenseViewModel>>(expenses);

            return PartialView("_BillTable", expensesViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAsync(ExpenseViewModel expenseVM)
        {
            if (expenseVM.Id == Guid.Empty)
            {
                return Json(new
                {
                    success = false,
                    errors = new List<string> { "Invalid expense ID." }
                });
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                                       .SelectMany(v => v.Errors)
                                       .Select(e => e.ErrorMessage)
                                       .ToList();

                return Json(new { success = false, errors });
            }

            var entity = _mapper.Map<ExpenseViewModel, Expense>(expenseVM,
                new Dictionary<string, string>
                {
                    { "Currency", "Currency" },
                    { "Type", "Type" },
                    { "ValueString", "Value" },
                });

            var result = await _expenseService.UpdateExpenseAsync(entity);

            if (!result.Success)
                return Json(new { success = false, errors = result.Errors });

            TempData["SuccessMessage"] = "Expense updated successfully!";

            return Json(new { success = true });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var expense = await _expenseService.DeleteExpenseAsync(id);

            if (expense is null) return RedirectToAction("Index");

            TempData["SuccessMessage"] = "Expense deleted successfully!";

            return RedirectToAction("Index");
        }

        private bool IsFilterViewModelEmpty(ExpenseFilterViewModel filterViewModel)
        {
            if (filterViewModel == null || !filterViewModel.Expenses.Any()
                && string.IsNullOrWhiteSpace(filterViewModel.NameContains)
                && string.IsNullOrWhiteSpace(filterViewModel.SourceContains)
                && filterViewModel.DateRangeStart == null
                && filterViewModel.DateRangeEnd == null
                && filterViewModel.ValueRangeStart == null
                && filterViewModel.ValueRangeEnd == null
                && filterViewModel.Month == null
                && filterViewModel.Currency == null
                && filterViewModel.Type == null) 
                return true;

            return false;
        }
    }
}