namespace ExpenseManager.Tests.Mapping.TestModels
{
    public sealed class Target
    {
        public decimal ConvertedAmount { get; set; }

        public string FormattedAmount { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string GetName() => Name;
    }
}
