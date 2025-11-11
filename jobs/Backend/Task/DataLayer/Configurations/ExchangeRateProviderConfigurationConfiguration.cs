using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class ExchangeRateProviderConfigurationConfiguration : IEntityTypeConfiguration<Entities.ExchangeRateProviderConfiguration>
{
    public void Configure(EntityTypeBuilder<Entities.ExchangeRateProviderConfiguration> builder)
    {
        builder.ToTable("ExchangeRateProviderConfiguration");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd();

        builder.Property(c => c.SettingKey)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.SettingValue)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.Created)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        // Unique constraint
        builder.HasIndex(c => new { c.ProviderId, c.SettingKey })
            .IsUnique()
            .HasDatabaseName("UQ_Provider_Setting");

        // Navigation properties
        builder.HasOne(c => c.Provider)
            .WithMany(p => p.Configurations)
            .HasForeignKey(c => c.ProviderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
