using ExpenseManager.Web.Mapping.Configurations;
using System;
using System.Collections.Generic;

namespace ExpenseManager.Web.Mapping.Contracts
{
    public interface IObjectMapper
    {
        TTarget Map<TSource, TTarget>(TSource source, Action<MappingOptions<TSource, TTarget>> optionsAction = null) where TTarget : new();

        List<TTarget> MapCollection<TSource, TTarget>(IEnumerable<TSource> source, Action<MappingOptions<TSource, TTarget>> optionsAction = null) where TTarget : new();
    }
}