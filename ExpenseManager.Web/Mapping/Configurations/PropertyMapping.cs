using System;
using System.Reflection;

namespace ExpenseManager.Web.Mapping.Configurations
{
    public class PropertyMapping<TSource, TTarget>
    {
        public PropertyInfo SourceProperty { get; init; }
        
        public PropertyInfo TargetProperty { get; init; }
        
        public Func<object, object> Converter { get; init; }
    }
}
