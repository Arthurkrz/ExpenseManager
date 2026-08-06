namespace ExpenseManager.Tests.Mapping.TestModels
{
    public sealed class Source
    {
        public decimal Amount { get; set; }

        public string Name { get; set; } = string.Empty;

        public decimal GetAmount() => Amount;
    }
}
