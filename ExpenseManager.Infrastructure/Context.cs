using ExpenseManager.Core.Entities;
using ExpenseManager.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManager.Infrastructure
{
    public class Context : DbContext
    {
        public DbSet<Expense> Expenses { get; set; }

        public Context(DbContextOptions<Context> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfiguration(new ExpenseConfiguration());
    }
}
