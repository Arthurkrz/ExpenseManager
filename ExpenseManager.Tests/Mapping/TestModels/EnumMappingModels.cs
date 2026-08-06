namespace ExpenseManager.Tests.Mapping.TestModels
{
    internal enum SourceStatus
    {
        Pending,
        Approved,
        Rejected
    }

    internal enum TargetStatus
    {
        Pending,
        Approved,
        Rejected
    }

    internal class EnumSource
    {
        public SourceStatus Status { get; set; }
    }

    internal class EnumTarget
    {
        public TargetStatus Status { get; set; }
    }

    internal class NullableEnumSource
    {
        public SourceStatus? Status { get; set; }
    }

    internal class NullableEnumTarget
    {
        public TargetStatus? Status { get; set; }
    }

    internal enum ExtendedSourceStatus
    {
        Pending,
        Approved,
        Rejected,
        Archived
    }

    internal enum ReducedTargetStatus
    {
        Pending,
        Approved,
        Rejected
    }

    internal class MissingEnumMemberSource
    {
        public ExtendedSourceStatus Status { get; set; }
    }

    internal class MissingEnumMemberTarget
    {
        public ReducedTargetStatus Status { get; set; }
    }

    internal class NestedEnumSource
    {
        public NestedEnumDetailsSource Details { get; set; }
    }

    internal class NestedEnumTarget
    {
        public NestedEnumDetailsTarget Details { get; set; }
    }

    internal class NestedEnumDetailsSource
    {
        public SourceStatus Status { get; set; }
    }

    internal class NestedEnumDetailsTarget
    {
        public TargetStatus Status { get; set; }
    }
}
