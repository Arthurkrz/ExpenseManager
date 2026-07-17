using ExpenseManager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseManager.Infrastructure.Configurations
{
    internal class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.ToTable("Expenses")
                   .HasKey(x => x.Id);

            builder.Property(x => x.Id)
                   .HasColumnType("uniqueidentifier")
                   .IsRequired();

            builder.Property(x => x.Name)
                   .HasColumnName("Name")
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(x => x.Currency)
                   .HasColumnName("Currency")
                   .IsRequired();

            builder.Property(x => x.Value)
                   .HasColumnName("Value")
                   .HasPrecision(9, 2)
                   .IsRequired();

            builder.Property(x => x.Type)
                   .HasColumnName("Type")
                   .IsRequired();

            builder.Property(x => x.ExpenseDate)
                   .HasColumnName("ExpenseDate")
                   .IsRequired();

            builder.Property(x => x.Source)
                   .HasColumnName("Source")
                   .HasMaxLength(50)
                   .IsRequired();
        }
    }
}
