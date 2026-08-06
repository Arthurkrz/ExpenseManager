using System;
using System.Collections.Generic;

namespace ExpenseManager.Web.Mapping.Utilities
{
    internal sealed class MappingContext
    {
        private readonly HashSet<object> _activeObjects =
            new(ReferenceEqualityComparer.Instance);

        public void Enter(object source, Type targetType)
        {
            if (source.GetType().IsValueType) return;
            if (_activeObjects.Add(source)) return;

            throw new InvalidOperationException(
                $"A circular reference was detected while mapping " +
                $"'{source.GetType().Name}' to '{targetType.Name}'.");
        }

        public void Exit(object source)
        {
            if (source.GetType().IsValueType) return;
            _activeObjects.Remove(source);
        }
    }
}
