using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class SystemConfigurationConfiguration : IEntityTypeConfiguration<SystemConfiguration>
{
    public void Configure(EntityTypeBuilder<SystemConfiguration> builder)
    {
        builder.ToTable("SystemConfiguration");

        builder.HasKey(c => c.Key);

        builder.Property(c => c.Key)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.Value)
            .IsRequired();

        builder.Property(c => c.Description)
            .HasMaxLength(500);

        builder.Property(c => c.DataType)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("String");

        builder.Property(c => c.Modified)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        // Check constraint
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_Config_DataType",
                "[DataType] IN ('String', 'Int', 'Bool', 'DateTime', 'Decimal')");
        });

        // Navigation properties
        builder.HasOne(c => c.ModifiedByUser)
            .WithMany(u => u.ModifiedConfigurations)
            .HasForeignKey(c => c.ModifiedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
