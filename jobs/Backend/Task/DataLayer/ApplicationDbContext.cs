using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSets for all entities
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<ExchangeRateProvider> ExchangeRateProviders { get; set; }
    public DbSet<ExchangeRate> ExchangeRates { get; set; }
    public DbSet<ExchangeRateProviderConfiguration> ExchangeRateProviderConfigurations { get; set; }
    public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ExchangeRateFetchLog> ExchangeRateFetchLogs { get; set; }
    public DbSet<ErrorLog> ErrorLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Remove SQL Server-specific constraints when using SQLite (in-memory database)
        var isSqlite = Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite";
        if (isSqlite)
        {
            // Remove default value SQL expressions and computed columns (SQLite doesn't support SQL Server functions)
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    // Remove default value SQL expressions
                    if (property.GetDefaultValueSql() != null)
                    {
                        property.SetDefaultValueSql(null);
                    }

                    // Remove computed columns that use SQL Server functions
                    var computedSql = property.GetComputedColumnSql();
                    if (computedSql != null &&
                        (computedSql.Contains("DATEDIFF") || computedSql.Contains("DATEADD") ||
                         computedSql.Contains("DATEPART") || computedSql.Contains("MILLISECOND")))
                    {
                        property.SetComputedColumnSql(null);
                    }
                }

                // Remove check constraints that use SQL Server functions
                var checkConstraints = entityType.GetCheckConstraints().ToList();
                foreach (var checkConstraint in checkConstraints)
                {
                    var sql = checkConstraint.Sql;
                    if (sql.Contains("GETDATE()") || sql.Contains("SYSDATETIMEOFFSET()") || sql.Contains("GETUTCDATE()"))
                    {
                        entityType.RemoveCheckConstraint(checkConstraint.Name);
                    }
                }
            }
        }
    }
}
