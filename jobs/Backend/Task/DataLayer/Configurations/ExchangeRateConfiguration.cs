using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class ExchangeRateConfiguration : IEntityTypeConfiguration<ExchangeRate>
{
    public void Configure(EntityTypeBuilder<ExchangeRate> builder)
    {
        builder.ToTable("ExchangeRate");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .ValueGeneratedOnAdd();

        builder.Property(r => r.Multiplier)
            .IsRequired();

        builder.Property(r => r.Rate)
            .IsRequired()
            .HasPrecision(19, 6);

        builder.Property(r => r.ValidDate)
            .IsRequired();

        builder.Property(r => r.Created)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        // Unique constraint
        builder.HasIndex(r => new { r.ProviderId, r.BaseCurrencyId, r.TargetCurrencyId, r.ValidDate })
            .IsUnique()
            .HasDatabaseName("UQ_Rate_Provider_Date");

        // Check constraints (handled by database)
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Rate_Positive", "[Rate] > 0");
            t.HasCheckConstraint("CK_Multiplier_Positive", "[Multiplier] > 0");
            t.HasCheckConstraint("CK_ValidDate_NotFuture", "[ValidDate] <= CAST(GETDATE() AS DATE)");
            t.HasCheckConstraint("CK_Different_Currencies", "[BaseCurrencyId] <> [TargetCurrencyId]");
        });

        // Navigation properties
        builder.HasOne(r => r.Provider)
            .WithMany(p => p.ExchangeRates)
            .HasForeignKey(r => r.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.BaseCurrency)
            .WithMany(c => c.BaseCurrencyRates)
            .HasForeignKey(r => r.BaseCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(r => r.TargetCurrency)
            .WithMany(c => c.TargetCurrencyRates)
            .HasForeignKey(r => r.TargetCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
