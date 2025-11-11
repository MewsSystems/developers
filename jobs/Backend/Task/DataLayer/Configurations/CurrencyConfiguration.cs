using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class CurrencyConfiguration : IEntityTypeConfiguration<Currency>
{
    public void Configure(EntityTypeBuilder<Currency> builder)
    {
        builder.ToTable("Currency");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.Code)
            .IsRequired()
            .HasMaxLength(3);

        builder.HasIndex(c => c.Code)
            .IsUnique();

        // Navigation properties
        builder.HasMany(c => c.ProvidersWithBaseCurrency)
            .WithOne(p => p.BaseCurrency)
            .HasForeignKey(p => p.BaseCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.BaseCurrencyRates)
            .WithOne(r => r.BaseCurrency)
            .HasForeignKey(r => r.BaseCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(c => c.TargetCurrencyRates)
            .WithOne(r => r.TargetCurrency)
            .HasForeignKey(r => r.TargetCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
