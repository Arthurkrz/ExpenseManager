using System;
using System.Collections;
using System.Reflection;

namespace ExpenseManager.Web.Mapping.Utilities
{
    public static class MappingTypeHelper
    {
        public static Type UnwrapNullable(Type type) =>
            Nullable.GetUnderlyingType(type) ?? type;

        public static bool IsAssignableDirectly(Type sourceType, Type targetType)
        {
            var actualSourceType = UnwrapNullable(sourceType);
            var actualTargetType = UnwrapNullable(targetType);

            return targetType.IsAssignableFrom(sourceType) &&
                !targetType.IsEnum &&
                !sourceType.IsEnum &&
                !IsNestedObjectCandidate(actualSourceType);
        }

        public static bool IsEnumMappingPossible(Type sourceType, Type targetType)
        {
            var actualSourceType = UnwrapNullable(sourceType);
            var actualTargetType = UnwrapNullable(targetType);

            return actualSourceType.IsEnum && 
                actualTargetType.IsEnum;
        }

        public static bool IsNestedClassMappingPossible(Type sourceType, Type targetType) =>
            IsNestedObjectCandidate(sourceType) && IsNestedObjectCandidate(targetType);

        public static object CreateTargetInstance(Type targetType)
        {
            var actualTargetType = UnwrapNullable(targetType);

            if (actualTargetType.IsInterface)
                throw new InvalidOperationException(
                    $"Cannot create nested target type " +
                    $"'{actualTargetType.FullName}' " +
                    $"because it is an interface.");

            if (actualTargetType.IsAbstract)
                throw new InvalidOperationException(
                    $"Cannot create nested target type " +
                    $"'{actualTargetType.FullName}' " +
                    $"because it is abstract.");

            var constructor = actualTargetType.GetConstructor(
                BindingFlags.Public | BindingFlags.Instance, 
                null, Type.EmptyTypes, null);

            if (constructor is null)
                throw new InvalidOperationException(
                    $"Cannot create nested target type " +
                    $"'{actualTargetType.FullName}' because it does not have " +
                    "a public parameterless constructor.");

            try { return constructor.Invoke(null); }

            catch (TargetInvocationException exception)
            {
                throw new TargetInvocationException(
                    $"The constructor for nested target type " +
                    $"'{actualTargetType.FullName}' threw an exception.",
                    exception.InnerException);
            }
        }

        private static bool IsNestedObjectCandidate(Type type)
        {
            var actualType = UnwrapNullable(type);

            if (actualType.IsValueType || 
                actualType == typeof(string) ||
                typeof(IEnumerable).IsAssignableFrom(actualType) ||
                typeof(Delegate).IsAssignableFrom(actualType))
                    return false;

            return true;
        }    
    }
}
