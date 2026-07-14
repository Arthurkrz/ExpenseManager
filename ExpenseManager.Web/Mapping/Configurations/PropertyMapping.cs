using System;
using System.Reflection;

namespace ExpenseManager.Web.Mapping.Configurations
{
    public class PropertyMapping<TSource, TTarget>
    {
        public PropertyInfo SourceProperty { get; set; }
        
        public PropertyInfo TargetProperty { get; set; }
        
        public Func<object, object> Converter { get; set; }
    }
}
