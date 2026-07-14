using ExpenseManager.Web.Mapping.Configurations;
using ExpenseManager.Web.Mapping.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

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

            return source.Select(item => MapObject(
                item, options)).ToList();
        }

        private static MappingOptions<TSource, TTarget> BuildOptions<TSource, TTarget>(Action<MappingOptions<TSource, TTarget>> optionsAction)
        {
            var options = new MappingOptions<TSource, TTarget>();
            optionsAction?.Invoke(options);

            return options;
        }

        private TTarget MapObject<TSource, TTarget>(TSource source, MappingOptions<TSource, TTarget> options) where TTarget : new()
        {
            var target = new TTarget();

            ApplyConfiguredMappings(source, target, options);
            ApplyConventionMappings(source, target, options);

            return target;
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

        private void ApplyConventionMappings<TSource, TTarget>(TSource source, TTarget target, MappingOptions<TSource, TTarget> options)
        {
            var sourceProps = typeof(TSource).GetProperties()
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            var targetProps = typeof(TTarget).GetProperties()
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            var alreadyMappedTargetProperties = options.PropertyMappings
                .Select(m => m.TargetProperty.Name)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            foreach (var sourceProp in sourceProps.Values)
            {
                if (!targetProps.TryGetValue(sourceProp.Name, out var targetProp)) continue;
                if (alreadyMappedTargetProperties.Contains(targetProp.Name)) continue;
                if (targetProp.SetMethod == null || !targetProp.SetMethod.IsPublic) continue;

                var sourceValue = sourceProp.GetValue(source);

                if (IsAssignableDirectly(sourceProp.PropertyType, targetProp.PropertyType))
                {
                    targetProp.SetValue(target, sourceValue);
                    continue;
                }

                if (IsEnumMappingPossible(sourceProp.PropertyType, targetProp.PropertyType))
                {
                    var mappedEnum = MapEnumValue(sourceValue,
                        sourceProp.PropertyType, targetProp.PropertyType);

                    targetProp.SetValue(target, mappedEnum);
                    continue;
                }

                if (IsNestedClassMappingPossible(sourceProp.PropertyType, targetProp.PropertyType))
                {
                    var mappedClass = MapNestedClass(sourceValue, sourceProp.PropertyType);

                    targetProp.SetValue(target, mappedClass);
                    continue;
                }
            }
        }

        private static object MapNestedClass(object sourceValue, Type targetType)
        {
            if (sourceValue is null) return null;

            var sourceType = sourceValue.GetType();
            object target = Activator.CreateInstance(targetType);

            var sourceProps = sourceType.GetProperties()
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            var targetProps = targetType.GetProperties()
                .ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            foreach (var sourceProp in sourceProps.Values)
            {
                if (!targetProps.TryGetValue(sourceProp.Name, out var targetProp)) continue;
                if (targetProp.SetMethod == null || !targetProp.SetMethod.IsPublic) continue;

                var sourcePropValue = sourceProp.GetValue(sourceValue);

                if (IsAssignableDirectly(sourceProp.PropertyType, targetProp.PropertyType))
                {
                    targetProp.SetValue(target, sourceValue);
                    continue;
                }

                if (IsEnumMappingPossible(sourceProp.PropertyType, targetProp.PropertyType))
                {
                    var mappedEnum = MapEnumValue(sourceValue,
                        sourceProp.PropertyType, targetProp.PropertyType);

                    targetProp.SetValue(target, mappedEnum);
                    continue;
                }

                if (IsNestedClassMappingPossible(sourceProp.PropertyType, targetProp.PropertyType))
                {
                    var mappedClass = MapNestedClass(sourceValue, sourceProp.PropertyType);

                    targetProp.SetValue(target, mappedClass);
                    continue;
                }
            }

            return target;
        }

        private static object MapEnumValue(object sourceValue, Type sourceType, Type targetType)
        {
            if (sourceValue is null) return null;

            var sourceEnumType = Nullable.GetUnderlyingType(sourceType) ?? sourceType;
            var targetEnumType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            var enumName = Enum.GetName(sourceEnumType, sourceValue);

            if (enumName is null) return null;

            return Enum.Parse(targetEnumType, enumName);
        }

        private static bool IsEnumMappingPossible(Type sourceType, Type targetType)
        {
            var sourceEnumType = Nullable.GetUnderlyingType(sourceType) ?? sourceType;
            var targetEnumType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            return sourceEnumType.IsEnum && targetEnumType.IsEnum;
        }

        private static bool IsAssignableDirectly(Type sourceType, Type targetType) =>
            targetType.IsAssignableFrom(sourceType) && 
                !targetType.IsEnum &&
                !sourceType.IsEnum && 
                !IsComplexClass(sourceType);

        private static bool IsNestedClassMappingPossible(Type sourceType, Type targetType) =>
            IsComplexClass(sourceType) && IsComplexClass(targetType);

        private static bool IsComplexClass(Type type) =>
            type.IsClass && type != typeof(string);
    }
}