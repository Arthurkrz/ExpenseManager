using ExpenseManager.Core.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace ExpenseManager.Web.Mapping
{
    public class MappingProfile : IMap
    {
        public TTarget Map<TSource, TTarget>(TSource source, Dictionary<string, string> mappingProps = null) where TTarget : new()
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source), 
                    "Object not identified.");

            var target = new TTarget();
            Type sourceType = typeof(TSource);
            Type targetType = typeof(TTarget);

            var sourceProps = sourceType.GetProperties()
                .ToDictionary(p => p.Name.ToLower(), p => p);

            var targetProps = targetType.GetProperties()
                .ToDictionary(p => p.Name.ToLower(), p => p);

            foreach (var sourceProp in sourceProps.Values)
            {
                string targetPropName = mappingProps is not null && 
                    mappingProps.ContainsKey(sourceProp.Name)
                     ? mappingProps[sourceProp.Name] : sourceProp.Name;

                targetPropName = targetPropName.ToLower();

                if (!targetProps.ContainsKey(targetPropName)) continue;

                var targetProp = targetProps[targetPropName];

                if (targetProp.SetMethod == null || 
                    !targetProp.SetMethod.IsPublic) continue;

                if (mappingProps != null && mappingProps.ContainsKey(sourceProp.Name))
                {
                    var mappedSpecial = SpecialMap(source, sourceProp, targetProp);
                    targetProp.SetValue(target, mappedSpecial);
                    continue;
                }

                if (targetProp.PropertyType == sourceProp.PropertyType && 
                    !targetProp.PropertyType.IsClass && 
                    !targetProp.PropertyType.IsEnum)
                {
                    object value = sourceProp.GetValue(source);
                    targetProp.SetValue(target, value);
                    continue;
                }

                else if (targetProp.PropertyType.IsEnum || Nullable.GetUnderlyingType(
                    targetProp.PropertyType)?.IsEnum == true)
                {
                    var targetEnumType = Nullable.GetUnderlyingType(
                        targetProp.PropertyType) ?? targetProp.PropertyType;

                    var sourceEnumType = Nullable.GetUnderlyingType(
                        sourceProp.PropertyType) ?? sourceProp.PropertyType;

                    var sourceValue = sourceProp.GetValue(source);
                    if (sourceValue is not null)
                    {
                        var enumName = Enum.GetName(sourceEnumType, sourceValue);

                        var targetValue = Enum.Parse(targetEnumType, enumName);

                        var boxedTargetValue = Convert.ChangeType(targetValue, targetEnumType);
                        targetProp.SetValue(target, boxedTargetValue);
                    }

                    else targetProp.SetValue(target, null);
                    continue;
                }

                else if (targetProp.PropertyType == typeof(string) && 
                    sourceProp.PropertyType == typeof(string))
                {
                    object value = sourceProp.GetValue(source);
                    targetProp.SetValue(target, value);
                    continue;
                }

                else if (targetProp.PropertyType.IsClass && 
                         sourceProp.PropertyType.IsClass && 
                         targetProp.PropertyType != typeof(string))
                {
                    var sourceClassProp = sourceProp.GetValue(source);
                    var mappedClass = ClassMap(sourceClassProp, targetProp.PropertyType);
                    targetProp.SetValue(target, mappedClass);
                }
            }

            return target;
        }

        private object ClassMap(object source, Type targetType)
        {
            if (source is null) return null;

            object target = Activator.CreateInstance(targetType);

            var sourceProps = source.GetType().GetProperties()
                .ToDictionary(p => p.Name.ToLower(), p => p);

            var targetProps = targetType.GetProperties()
                .ToDictionary(p => p.Name.ToLower(), p => p);

            foreach(var sourceProp in sourceProps.Values)
            {
                if (!targetProps.ContainsKey(sourceProp.Name)) continue;

                var targetProp = targetProps[sourceProp.Name];

                if (targetProp.PropertyType == sourceProp.PropertyType &&
                   !targetProp.PropertyType.IsClass &&
                   !targetProp.PropertyType.IsEnum)
                {
                    object value = sourceProp.GetValue(source);
                    targetProp.SetValue(target, value);
                }

                else if (targetProp.PropertyType.IsClass &&
                         sourceProp.PropertyType.IsClass &&
                         targetProp.PropertyType != typeof(string))
                {
                    var sourcePropValue = sourceProp.GetValue(source);
                    var mappedValue = ClassMap(sourcePropValue, targetProp.PropertyType);
                    targetProp.SetValue(target, mappedValue);
                }
            }

            return target;
        }

        private object SpecialMap(object source, PropertyInfo sourceProp, PropertyInfo targetProp)
        {
            if (source == null) return null;

            string sourceName = sourceProp.Name;
            string targetName = targetProp.Name;

            if (sourceName == "Value" && targetName == "ValueString")
            {
                double value = (double)sourceProp.GetValue(source);
                return value.ToString("F2", CultureInfo.InvariantCulture);
            }
                
            if (sourceName == "ValueString" && targetName == "Value")
            {
                string valueString = (string)sourceProp.GetValue(source);

                if (double.TryParse(valueString, NumberStyles.Any, 
                    CultureInfo.InvariantCulture, out double result))
                        return result;

                return 0.0;
            }

            if (sourceName == "ValueRangeStart" && 
                targetName == "ValueStringRangeStart")
            {
                double value = (double)sourceProp.GetValue(source);
                return value.ToString("F2", CultureInfo.InvariantCulture);
            }

            if (sourceName == "ValueRangeEnd" && 
                targetName == "ValueStringRangeEnd")
            {
                double value = (double)sourceProp.GetValue(source);
                return value.ToString("F2", CultureInfo.InvariantCulture);
            }

            if (sourceName == "ValueStringRangeStart" && 
                targetName == "ValueRangeStart")
            {
                string valueString = (string)sourceProp.GetValue(source);

                if (double.TryParse(valueString, NumberStyles.Any, 
                    CultureInfo.InvariantCulture, out double result))
                        return result;

                return 0.0;
            }

            if (sourceName == "ValueStringRangeEnd" && targetName == "ValueRangeEnd")
            {
                string valueString = (string)sourceProp.GetValue(source);

                if (double.TryParse(valueString, NumberStyles.Any,
                    CultureInfo.InvariantCulture, out double result))
                        return result;

                return 0.0;
            }

            return sourceProp.GetValue(source);
        }
    }
}