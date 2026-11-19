using ExpenseManager.Web.Mapping.Configurations;
using ExpenseManager.Web.Mapping.Contracts;
using ExpenseManager.Web.Mapping.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExpenseManager.Web.Mapping
{
    public class ObjectMapper : IObjectMapper
    {
        public TTarget Map<TSource, TTarget>(TSource source, Action<MappingOptions<TSource, TTarget>> optionsAction = null) where TTarget : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            var options = BuildOptions(optionsAction);

            return MapObject(source, options);
        }

        public List<TTarget> MapCollection<TSource, TTarget>(IEnumerable<TSource> source, Action<MappingOptions<TSource, TTarget>> optionsAction = null) where TTarget : new()
        {
            ArgumentNullException.ThrowIfNull(source);

            var options = BuildOptions(optionsAction);

            return source.Select(item =>
            {
                ArgumentNullException.ThrowIfNull(item);
                return MapObject(item, options);
            }).ToList();
        }

        private static MappingOptions<TSource, TTarget> BuildOptions<TSource, TTarget>(Action<MappingOptions<TSource, TTarget>> optionsAction)
        {
            var options = new MappingOptions<TSource, TTarget>();
            optionsAction?.Invoke(options);

            return options;
        }

        private TTarget MapObject<TSource, TTarget>(TSource source, MappingOptions<TSource, TTarget> options) where TTarget : new()
        {
            var context = new MappingContext();

            context.Enter(source!, typeof(TTarget));

            try
            {
                var target = new TTarget();

                ApplyConfiguredMappings(source, target, options);

                var alreadyMappedProperties = options.PropertyMappings
                    .Select(m => m.TargetProperty.Name)
                    .ToHashSet(StringComparer.OrdinalIgnoreCase);

                ApplyConventionMappings(source, target, 
                    alreadyMappedProperties, context);

                return target;
            }

            finally { context.Exit(source!); }
        }

        private void ApplyConfiguredMappings<TSource, TTarget>(TSource source, TTarget target, MappingOptions<TSource, TTarget> options)
        {
            foreach (var mapping in options.PropertyMappings)
            {
                var sourceValue = mapping.SourceProperty.GetValue(source);
                var convertedValue = mapping.Converter(sourceValue);

                mapping.TargetProperty.SetValue(target, convertedValue);
            }
        }

        private static void ApplyConventionMappings(object source, object target, HashSet<string> ignoredTargetProperties, MappingContext context)
        {
            var sourceProperties = GetReadableProperties(source.GetType());
            var targetProperties = GetWritableProperties(target.GetType());

            foreach (var sourceProperty in sourceProperties.Values)
            {
                if (!targetProperties.TryGetValue(sourceProperty.Name, 
                        out var targetProperty))
                            continue;

                if (ignoredTargetProperties.Contains(targetProperty.Name))
                    continue;

                MapProperty(source, target, sourceProperty, 
                    targetProperty, context);
            }
        }

        private static void ApplyNestedConventionMappings(object source, object target, MappingContext context)
        {
            var sourceProperties = GetReadableProperties(source.GetType());
            var targetProperties = GetWritableProperties(target.GetType());

            foreach (var sourceProperty in sourceProperties.Values)
            {
                if (!targetProperties.TryGetValue(sourceProperty.Name, 
                    out var targetProperty))
                        continue;

                MapProperty(source, target, sourceProperty, 
                    targetProperty, context);
            }
        }

        private static void MapProperty(object source, object target, PropertyInfo sourceProperty, PropertyInfo targetProperty, MappingContext context)
        {
            var sourceValue = sourceProperty.GetValue(source);

            if (MappingTypeHelper.IsAssignableDirectly(
                    sourceProperty.PropertyType,
                    targetProperty.PropertyType))
            {
                targetProperty.SetValue(target, sourceValue);
                return;
            }

            if (MappingTypeHelper.IsEnumMappingPossible(
                    sourceProperty.PropertyType,
                    targetProperty.PropertyType))
            {
                var mappedEnum = MapEnumValue(sourceValue, 
                    sourceProperty.PropertyType, 
                    targetProperty.PropertyType);

                targetProperty.SetValue(target, mappedEnum);
                return;
            }

            if (MappingTypeHelper.IsNestedClassMappingPossible(
                    sourceProperty.PropertyType,
                    targetProperty.PropertyType))
            {
                var mappedObject = MapNestedObject(sourceValue, 
                    targetProperty.PropertyType, context);

                targetProperty.SetValue(target, mappedObject);
            }
        }

        private static object MapNestedObject(object sourceValue, Type targetType, MappingContext context)
        {
            if (sourceValue is null) return null;

            context.Enter(sourceValue, targetType);

            try
            {
                var target = MappingTypeHelper.CreateTargetInstance(targetType);
                ApplyNestedConventionMappings(sourceValue, target, context);

                return target;
            }

            finally { context.Exit(sourceValue); }
        }

        private static object MapEnumValue(object sourceValue, Type sourceType, Type targetType)
        {
            var sourceEnumType = MappingTypeHelper.UnwrapNullable(sourceType);
            var targetEnumType = MappingTypeHelper.UnwrapNullable(targetType);

            if (sourceValue is null)
            {
                var targetIsNullable = Nullable.GetUnderlyingType(targetType) is not null;

                if (!targetIsNullable)
                    throw new InvalidOperationException(
                        $"Cannot map a null value from enum " +
                        $"'{sourceEnumType.Name}' to non-nullable enum " +
                        $"'{targetEnumType.Name}'.");

                return null;
            }

            var enumName = Enum.GetName(sourceEnumType, sourceValue) ??
                throw new InvalidOperationException($"The value " +
                    $"'{sourceValue}' is not defined in enum " +
                    $"'{sourceEnumType.Name}'.");

            if (!Enum.TryParse(targetEnumType, enumName,
                ignoreCase: false, out var mappedValue))
                    throw new InvalidOperationException(
                        $"Cannot map enum value " +
                        $"'{sourceEnumType.Name}.{enumName}' to enum " +
                        $"'{targetEnumType.Name}' because the target enum " +
                        $"does not contain a member named '{enumName}'.");

            return mappedValue;
        }

        private static Dictionary<string, PropertyInfo> GetReadableProperties(Type type) =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.CanRead &&
                    property.GetMethod?.IsPublic == true &&
                    property.GetIndexParameters().Length == 0)
                .ToDictionary(property => property.Name, property => property,
                    StringComparer.OrdinalIgnoreCase);

        private static Dictionary<string, PropertyInfo> GetWritableProperties(Type type) =>
            type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(property => property.CanWrite &&
                    property.SetMethod?.IsPublic == true &&
                    property.GetIndexParameters().Length == 0)
                .ToDictionary(property => property.Name, property => property, 
                    StringComparer.OrdinalIgnoreCase);
    }
}