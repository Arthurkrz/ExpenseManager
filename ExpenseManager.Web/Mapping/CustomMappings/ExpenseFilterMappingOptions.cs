using ExpenseManager.Core.Entities;
using ExpenseManager.Web.Mapping.Configurations;
using ExpenseManager.Web.Models;

namespace ExpenseManager.Web.Mapping.CustomMappings
{
    public static class ExpenseFilterMappingOptions
    {
        public static MappingOptions<ExpenseFilterViewModel, ExpenseFilter> MapViewModelToFilter(this MappingOptions<ExpenseFilterViewModel, ExpenseFilter> options) =>
            options.MapProperty
            (
                source => source.ValueStringRangeStart,
                target => target.ValueRangeStart,
                MappingConverters.StringToNullableMoney
            )
            .MapProperty
            (
                source => source.ValueStringRangeEnd,
                target => target.ValueRangeEnd,
                MappingConverters.StringToNullableMoney
            );
    }
}
