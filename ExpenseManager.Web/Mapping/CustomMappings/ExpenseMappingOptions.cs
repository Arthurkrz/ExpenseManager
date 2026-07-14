using ExpenseManager.Core.Entities;
using ExpenseManager.Web.Mapping.Configurations;
using ExpenseManager.Web.Models;

namespace ExpenseManager.Web.Mapping.CustomMappings
{
    public static class ExpenseMappingOptions
    {
        public static MappingOptions<Expense, ExpenseViewModel> MapExpenseToViewModel(this MappingOptions<Expense, ExpenseViewModel> options) =>
            options.MapProperty
            (
                source => source.Value,
                target => target.ValueString,
                MappingConverters.MoneyToString
            );

        public static MappingOptions<ExpenseViewModel, Expense> MapViewModelToExpense(this MappingOptions<ExpenseViewModel, Expense> options) =>
            options.MapProperty
            (
                source => source.ValueString,
                target => target.Value,
                MappingConverters.StringToMoney
            );
    }
}
