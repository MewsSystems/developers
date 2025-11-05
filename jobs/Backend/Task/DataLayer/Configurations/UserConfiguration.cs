using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataLayer.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("User");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedOnAdd();

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(255);

        builder.HasIndex(u => u.Email)
            .IsUnique();

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasMaxLength(512);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasMaxLength(255);

        // Check constraint
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_User_Role", "[Role] IN ('Admin', 'Consumer')");
        });

        // Navigation properties
        builder.HasMany(u => u.ModifiedConfigurations)
            .WithOne(c => c.ModifiedByUser)
            .HasForeignKey(c => c.ModifiedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.FetchLogs)
            .WithOne(l => l.RequestedByUser)
            .HasForeignKey(l => l.RequestedBy)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasMany(u => u.ErrorLogs)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
