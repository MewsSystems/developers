using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class ErrorLogConfiguration : IEntityTypeConfiguration<ErrorLog>
{
    public void Configure(EntityTypeBuilder<ErrorLog> builder)
    {
        builder.ToTable("ErrorLog");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Timestamp)
            .IsRequired()
            .HasDefaultValueSql("SYSDATETIMEOFFSET()");

        builder.Property(e => e.Severity)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.Source)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Message)
            .IsRequired();

        // Check constraint
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_ErrorLog_Severity",
                "[Severity] IN ('Info', 'Warning', 'Error', 'Critical')");
        });

        // Navigation properties
        builder.HasOne(e => e.User)
            .WithMany(u => u.ErrorLogs)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
