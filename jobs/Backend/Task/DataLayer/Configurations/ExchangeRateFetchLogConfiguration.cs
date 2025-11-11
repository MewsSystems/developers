using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class ExchangeRateFetchLogConfiguration : IEntityTypeConfiguration<ExchangeRateFetchLog>
{
    public void Configure(EntityTypeBuilder<ExchangeRateFetchLog> builder)
    {
        builder.ToTable("ExchangeRateFetchLog");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .ValueGeneratedOnAdd();

        builder.Property(l => l.FetchStarted)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.Property(l => l.Status)
            .IsRequired()
            .HasMaxLength(20)
            .HasDefaultValue("Running");

        // Computed column
        builder.Property(l => l.DurationMs)
            .HasComputedColumnSql("DATEDIFF(MILLISECOND, [FetchStarted], [FetchCompleted])", stored: true);

        // Check constraint
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_FetchLog_Status",
                "[Status] IN ('Running', 'Success', 'Failed', 'PartialSuccess')");
        });

        // Navigation properties
        builder.HasOne(l => l.Provider)
            .WithMany(p => p.FetchLogs)
            .HasForeignKey(l => l.ProviderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.RequestedByUser)
            .WithMany(u => u.FetchLogs)
            .HasForeignKey(l => l.RequestedBy)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
