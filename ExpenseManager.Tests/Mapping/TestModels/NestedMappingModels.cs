using System;

namespace ExpenseManager.Tests.Mapping.TestModels
{
    internal class NestedSource
    {
        public string Name { get; set; } = string.Empty;
        public NestedAddressSource Address { get; set; }
    }

    internal class NestedTarget
    {
        public string Name { get; set; } = string.Empty;
        public NestedAddressTarget Address { get; set; }
    }

    internal class NestedTargetThrowException
    {
        public string Name { get; set; } = string.Empty;
        public NestedAddressTargetThrowException Adress { get; set; }
    }

    internal class NestedAddressTargetThrowException
    {
        public NestedAddressTargetThrowException()
        {
            throw new Exception("Error");
        }

        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    internal class NestedAddressSource
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    internal class NestedAddressTarget
    {
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
    }

    internal class NestedIncompatibleSource
    {
        public NestedIncompatibleValueSource Value { get; set; }
    }

    internal class NestedIncompatibleTarget
    {
        public NestedIncompatibleValueTarget Value { get; set; }
    }

    internal class NestedIncompatibleValueSource
    {
        public string Amount { get; set; } = string.Empty;
    }

    internal class NestedIncompatibleValueTarget
    {
        public decimal Amount { get; set; }
    }

    internal class NestedPrivateSetterSource
    {
        public NestedPrivateSetterValueSource Value { get; set; }
    }

    internal class NestedPrivateSetterTarget
    {
        public NestedPrivateSetterValueTarget Value { get; private set; }
    }

    internal class NestedPrivateSetterValueSource
    {
        public string Description { get; set; } = string.Empty;
    }

    internal class NestedPrivateSetterValueTarget
    {
        public string Description { get; set; } = string.Empty;
    }

    internal class CyclicSource
    {
        public CyclicSource Parent { get; set; }
    }

    internal class CyclicTarget
    {
        public CyclicTarget Parent { get; set; }
    }

    internal class ConstructibleSource
    {
        public ConstructibleDetailsSource Details { get; set; }
    }

    internal class ConstructibleDetailsSource
    {
        public string Name { get; set; } = string.Empty;
    }

    internal class TargetWithoutDefaultConstructor
    {
        public TargetDetailsWithoutDefaultConstructor Details { get; set; }
    }

    internal class TargetDetailsWithoutDefaultConstructor
    {
        public TargetDetailsWithoutDefaultConstructor(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
    }

    internal class NestedMissingPropertySource
    {
        public NestedMissingPropertyDetailsSource Details { get; set; }
    }

    internal class NestedMissingPropertyTarget
    {
        public NestedMissingPropertyDetailsTarget Details { get; set; }
    }

    internal class NestedMissingPropertyDetailsSource
    {
        public string Name { get; set; } = string.Empty;

        public string SourceOnly { get; set; } = string.Empty;
    }

    internal class NestedMissingPropertyDetailsTarget
    {
        public string Name { get; set; } = string.Empty;
    }
}
