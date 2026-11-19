using System;

namespace ExpenseManager.Tests.Services.Cache
{
    public sealed class CachedExpense
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public decimal Value { get; set; }
    }
}
