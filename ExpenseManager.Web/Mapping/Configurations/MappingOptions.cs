using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpenseManager.Web.Mapping.Configurations
{
    public sealed class MappingOptions<TSource, TTarget>
    {
        public List<PropertyMapping<TSource, TTarget>> PropertyMappings { get; } = [];

        public MappingOptions<TSource, TTarget> MapProperty<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceExpression, Expression<Func<TTarget, TTargetProperty>> targetExpression, Func<TSourceProperty, TTargetProperty> converter)
        {
            ArgumentNullException.ThrowIfNull(sourceExpression);
            ArgumentNullException.ThrowIfNull(targetExpression);
            ArgumentNullException.ThrowIfNull(converter);

            var sourceProperty = GetPropertyInfo(sourceExpression);
            var targetProperty = GetPropertyInfo(targetExpression);

            if (targetProperty.SetMethod?.IsPublic != true)
                throw new InvalidOperationException(
                    $"Target property '{targetProperty.Name}' " +
                    $"must have a public setter.");

            PropertyMappings.Add(new PropertyMapping<TSource, TTarget>
            {
                SourceProperty = sourceProperty,
                TargetProperty = targetProperty,
                Converter = value =>
                {
                    if (value is null) return converter(default!);
                    return converter((TSourceProperty)value);
                }
            });

            return this;
        }

        private static PropertyInfo GetPropertyInfo<TObject, TProperty>(
            Expression<Func<TObject, TProperty>> expression)
        {
            if (expression.Body is MemberExpression
                { Member: PropertyInfo property })
                    return property;

            if (expression.Body is UnaryExpression
                { Operand: MemberExpression 
                    { Member: PropertyInfo convertedProperty } })
                        return convertedProperty;

            throw new ArgumentException(
                "Expression must point to a property.");
        }
    }
}
