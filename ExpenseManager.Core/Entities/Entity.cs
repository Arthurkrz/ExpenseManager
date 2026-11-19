using System;

namespace ExpenseManager.Core.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
    }
}