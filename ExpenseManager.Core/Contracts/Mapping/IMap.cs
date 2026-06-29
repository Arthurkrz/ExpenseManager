using System.Collections.Generic;

namespace ExpenseManager.Core.Contracts.Mapping
{
    public interface IMap
    {
        TTarget Map<TSource, TTarget>(TSource source, Dictionary<string, string> propMappings = null) where TTarget : new();
    }
}