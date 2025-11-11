using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class ExchangeRateProviderConfiguration : IEntityTypeConfiguration<ExchangeRateProvider>
{
    public void Configure(EntityTypeBuilder<ExchangeRateProvider> builder)
    {
        builder.ToTable("ExchangeRateProvider");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(10);

        builder.HasIndex(p => p.Code)
            .IsUnique();

        builder.Property(p => p.Url)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(p => p.RequiresAuthentication)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(p => p.ApiKeyVaultReference)
            .HasMaxLength(255);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(p => p.ConsecutiveFailures)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(p => p.Created)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        // Navigation properties
        builder.HasOne(p => p.BaseCurrency)
            .WithMany(c => c.ProvidersWithBaseCurrency)
            .HasForeignKey(p => p.BaseCurrencyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.ExchangeRates)
            .WithOne(r => r.Provider)
            .HasForeignKey(r => r.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Configurations)
            .WithOne(c => c.Provider)
            .HasForeignKey(c => c.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.FetchLogs)
            .WithOne(l => l.Provider)
            .HasForeignKey(l => l.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
