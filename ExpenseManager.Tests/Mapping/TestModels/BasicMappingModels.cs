using System.Collections.Generic;

namespace ExpenseManager.Tests.Mapping.TestModels
{
    internal class BasicSource
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    internal class BasicTarget
    {
        public string Name { get; set; } = string.Empty;
        public int Age { get; set; }
    }

    internal class CaseInsensitiveSource
    {
        public string NAME { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    internal class CaseInsensitiveTarget
    {
        public string Name { get; set; } = string.Empty;
        public int Quantity { get; set; }
    }

    internal class MissingPropertySource
    {
        public string Name { get; set; } = string.Empty;
        public string SourceOnlyProperty { get; set; } = string.Empty;
    }

    internal class MissingPropertyTarget
    {
        public string Name { get; set; } = string.Empty;
    }

    internal class IncompatibleSource
    {
        public int Value { get; set; }
    }

    internal class IncompatibleTarget
    {
        public string Value { get; set; } = string.Empty;
    }

    internal class PrivateSetterSource
    {
        public string Name { get; private set; } = string.Empty;
    }

    internal class PrivateSetterTarget
    {
        public string Name { get; private set; } = string.Empty;
    }

    internal class NoSetterSource
    {
        public string Name { get; } = string.Empty;
    }

    internal class NoSetterTarget
    {
        public string Name { get; } = string.Empty;
    }

    internal class CustomMappingSource
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    internal class CustomMappingTarget
    {
        public string FullName { get; set; } = string.Empty;
    }

    internal abstract class AbstractSource
    {
        public string Name { get; set; } = string.Empty;
    }

    internal abstract class AbstractTarget
    {
        public string Name { get; set; } = string.Empty;
    }

    internal interface InterfaceSource { }

    internal interface InterfaceTarget { }
}
