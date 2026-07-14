using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpenseManager.Web.Mapping.Configurations
{
    public class MappingOptions<TSource, TTarget>
    {
        private readonly List<PropertyMapping<TSource, TTarget>> _propertyMappings = [];

        public IReadOnlyList<PropertyMapping<TSource, TTarget>> PropertyMappings => _propertyMappings;

        public MappingOptions<TSource, TTarget> MapProperty<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, Func<TSourceProperty, TTargetProperty> converter)
        {
            var sourcePropertyInfo = GetPropertyInfo(sourceProperty);
            var targetPropertyInfo = GetPropertyInfo(targetProperty);

            _propertyMappings.Add(new PropertyMapping<TSource, TTarget>
            { 
                SourceProperty = sourcePropertyInfo,
                TargetProperty = targetPropertyInfo,
                Converter = value => converter((TSourceProperty)value)
            });

            return this;
        }

        private static PropertyInfo GetPropertyInfo<TObject, TProperty>(Expression<Func<TObject, TProperty>> expression)
        {
            if (expression.Body is MemberExpression memberExpression &&
                memberExpression.Member is PropertyInfo propertyInfo)
                    return propertyInfo;

            throw new ArgumentException("Expression must point to a property.");
        }
    }
}
