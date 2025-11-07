using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DataLayer.Seeding;

/// <summary>
/// Seeds the database with initial data when using in-memory database.
/// Based on seed scripts from Database/Script directory.
/// </summary>
public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(ApplicationDbContext context, ILogger<DatabaseSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Seeds all required data for the application to function.
    /// </summary>
    public async Task SeedAllAsync()
    {
        _logger.LogInformation("Starting database seeding...");

        await SeedCurrenciesAsync();
        await SeedUsersAsync();
        await SeedSystemConfigurationAsync();
        await SeedExchangeRateProvidersAsync();
        await CreateViewsAsync();

        _logger.LogInformation("Database seeding completed successfully.");
    }

    /// <summary>
    /// Seeds currencies: CZK, EUR, RON
    /// Based on: Database/Script/SeedCurrencies.sql
    /// </summary>
    private async Task SeedCurrenciesAsync()
    {
        _logger.LogInformation("Seeding currencies...");

        var currencies = new[]
        {
            new Currency { Code = "CZK" },
            new Currency { Code = "EUR" },
            new Currency { Code = "RON" }
        };

        foreach (var currency in currencies)
        {
            if (!await _context.Currencies.AnyAsync(c => c.Code == currency.Code))
            {
                _context.Currencies.Add(currency);
                _logger.LogInformation("  ✓ {CurrencyCode} created", currency.Code);
            }
            else
            {
                _logger.LogInformation("  ⚠ {CurrencyCode} already exists", currency.Code);
            }
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds default users: admin and consumer
    /// Based on: Database/Script/SeedUsers.sql
    /// </summary>
    private async Task SeedUsersAsync()
    {
        _logger.LogInformation("Seeding users...");

        // BCrypt hash for password "simple" (cost factor 11)
        const string passwordHash = "$2a$11$i0UZQX8ZlJ2yY6DuZ4Y0HO0efKgQjvFMeMFv7wYJUVSIqmuzKSBJ.";

        var users = new[]
        {
            new User
            {
                Email = "admin@example.com",
                PasswordHash = passwordHash,
                FirstName = "Admin",
                LastName = "User",
                Role = "Admin"
            },
            new User
            {
                Email = "consumer@example.com",
                PasswordHash = passwordHash,
                FirstName = "Consumer",
                LastName = "User",
                Role = "Consumer"
            }
        };

        foreach (var user in users)
        {
            if (!await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                _context.Users.Add(user);
                _logger.LogInformation("  ✓ {Email} user created", user.Email);
            }
            else
            {
                _logger.LogInformation("  ⚠ {Email} user already exists", user.Email);
            }
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds system configuration settings
    /// Based on: Database/Script/SeedSystemConfiguration.sql
    /// </summary>
    private async Task SeedSystemConfigurationAsync()
    {
        _logger.LogInformation("Seeding system configuration...");

        var now = DateTimeOffset.UtcNow;

        var configurations = new[]
        {
            new SystemConfiguration { Key = "AutoDisableProviderAfterFailures", Value = "10", Description = "Auto-disable provider after N consecutive failures (0 = never)", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "ProviderHealthCheckIntervalMinutes", Value = "15", Description = "How often to check provider health (in minutes)", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "StaleDataThresholdHours", Value = "48", Description = "Consider data stale after N hours without update", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "RetainExchangeRatesDays", Value = "730", Description = "Keep exchange rates for N days (730 = 2 years)", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "RetainFetchLogsDays", Value = "90", Description = "Keep fetch logs for N days (90 = 3 months)", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "RetainErrorLogsDays", Value = "30", Description = "Keep error logs for N days (30 = 1 month)", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "EnableAutoCleanup", Value = "true", Description = "Automatically cleanup old data based on retention settings", DataType = "Bool", Modified = now },
            new SystemConfiguration { Key = "LogLevel", Value = "Information", Description = "Minimum log level: Debug, Information, Warning, Error, Critical", DataType = "String", Modified = now },
            new SystemConfiguration { Key = "EnableDetailedLogging", Value = "false", Description = "Enable detailed request/response logging (verbose)", DataType = "Bool", Modified = now },
            new SystemConfiguration { Key = "LogSuccessfulFetches", Value = "true", Description = "Log successful fetch operations", DataType = "Bool", Modified = now },
            new SystemConfiguration { Key = "ApiRateLimitPerMinute", Value = "60", Description = "Max API requests per minute per user", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "EnableApiKeyAuthentication", Value = "false", Description = "Require API key for public endpoints", DataType = "Bool", Modified = now },
            new SystemConfiguration { Key = "MaxResultsPerPage", Value = "100", Description = "Maximum results per page in API responses", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "SystemVersion", Value = "1.0.0", Description = "Current system version", DataType = "String", Modified = now },
            new SystemConfiguration { Key = "MaintenanceMode", Value = "false", Description = "Enable maintenance mode (blocks API access)", DataType = "Bool", Modified = now },
            new SystemConfiguration { Key = "MaintenanceMessage", Value = "System is under maintenance. Please try again later.", Description = "Message to display during maintenance", DataType = "String", Modified = now },
            new SystemConfiguration { Key = "HistoricalDataDays", Value = "90", Description = "Number of days of historical data to fetch on startup", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "DefaultRetryDelayMinutes", Value = "30", Description = "Default delay (in minutes) before retrying a failed fetch", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "MaxRetries", Value = "3", Description = "Maximum number of retry attempts for a failed fetch", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "HealthCheckIntervalMinutes", Value = "5", Description = "Interval (in minutes) between provider health checks", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "RecentDataThresholdHours", Value = "2", Description = "Time (in hours) to consider existing data as recent and schedule a retry", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "HangfireWorkerCount", Value = "5", Description = "Number of concurrent Hangfire background job workers", DataType = "Int", Modified = now },
            new SystemConfiguration { Key = "DefaultCronExpression", Value = "0 16 * * *", Description = "Default cron expression for scheduled fetches (daily at 4 PM UTC)", DataType = "String", Modified = now },
            new SystemConfiguration { Key = "DefaultTimezone", Value = "UTC", Description = "Default timezone for cron expressions", DataType = "String", Modified = now }
        };

        foreach (var config in configurations)
        {
            if (!await _context.SystemConfigurations.AnyAsync(c => c.Key == config.Key))
            {
                _context.SystemConfigurations.Add(config);
                _logger.LogInformation("  ✓ {ConfigKey}", config.Key);
            }
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Seeds exchange rate providers and their configurations
    /// Based on: Database/Script/SeedExchangeRateProvidersAndConfiguration.sql
    /// </summary>
    private async Task SeedExchangeRateProvidersAsync()
    {
        _logger.LogInformation("Seeding exchange rate providers...");

        // Get currency IDs
        var czkId = await _context.Currencies.Where(c => c.Code == "CZK").Select(c => c.Id).FirstOrDefaultAsync();
        var eurId = await _context.Currencies.Where(c => c.Code == "EUR").Select(c => c.Id).FirstOrDefaultAsync();
        var ronId = await _context.Currencies.Where(c => c.Code == "RON").Select(c => c.Id).FirstOrDefaultAsync();

        if (czkId == 0 || eurId == 0 || ronId == 0)
        {
            _logger.LogError("Required currencies (CZK, EUR, RON) not found! Cannot seed providers.");
            return;
        }

        var now = DateTimeOffset.UtcNow;

        // CNB Provider
        await SeedProviderAsync(new ExchangeRateProvider
        {
            Name = "Czech National Bank",
            Code = "CNB",
            Url = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml",
            BaseCurrencyId = czkId,
            RequiresAuthentication = false,
            IsActive = true,
            Created = now
        }, new Dictionary<string, (string Value, string Description)>
        {
            ["Format"] = ("XML", "Response format (XML with custom schema)"),
            ["DecimalSeparator"] = ("Comma", "Uses comma (,) as decimal separator"),
            ["UpdateTime"] = ("14:30", "Daily update time (after 14:30 CET)"),
            ["TimeZone"] = ("CET", "Central European Time"),
            ["Encoding"] = ("UTF-8", "Character encoding"),
            ["XmlNamespace"] = ("http://www.cnb.cz/xsd/Filharmonie/modely/Denni_kurz/1.1", "CNB XML namespace"),
            ["HasVariableAmounts"] = ("true", "Uses variable amounts (mnozstvi: 1, 100, 1000)"),
            ["XmlStructure"] = ("kurzy/tabulka/radek", "Uses kurzy > tabulka > radek structure"),
            ["RootElement"] = ("kurzy", "Root XML element"),
            ["HasDateAttribute"] = ("true", "Contains date attribute in root"),
            ["HasSequenceNumber"] = ("true", "Contains sequence number"),
            ["HistoricalUrl"] = ("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml?date={date}", "URL pattern for historical data")
        });

        // ECB Provider
        await SeedProviderAsync(new ExchangeRateProvider
        {
            Name = "European Central Bank",
            Code = "ECB",
            Url = "https://www.ecb.europa.eu/stats/eurofxref/eurofxref-daily.xml",
            BaseCurrencyId = eurId,
            RequiresAuthentication = false,
            IsActive = true,
            Created = now
        }, new Dictionary<string, (string Value, string Description)>
        {
            ["Format"] = ("XML", "Response format (XML)"),
            ["DecimalSeparator"] = ("Dot", "Uses dot (.) as decimal separator"),
            ["UpdateTime"] = ("16:00", "Daily update time (around 16:00 CET)"),
            ["TimeZone"] = ("CET", "Central European Time"),
            ["XmlNamespace"] = ("http://www.gesmes.org/xml/2002-08-01", "ECB XML namespace"),
            ["HasVariableAmounts"] = ("false", "All rates use amount = 1"),
            ["XmlStructure"] = ("TripleNestedCube", "Uses triple nested Cube structure"),
            ["Historical90DaysUrl"] = ("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist-90d.xml", "URL for last 90 days of historical data"),
            ["HistoricalAllUrl"] = ("https://www.ecb.europa.eu/stats/eurofxref/eurofxref-hist.xml", "URL for all historical data")
        });

        // BNR Provider
        await SeedProviderAsync(new ExchangeRateProvider
        {
            Name = "Banca Națională a României",
            Code = "BNR",
            Url = "https://www.bnr.ro/nbrfxrates.xml",
            BaseCurrencyId = ronId,
            RequiresAuthentication = false,
            IsActive = true,
            Created = now
        }, new Dictionary<string, (string Value, string Description)>
        {
            ["Format"] = ("XML", "Response format (XML)"),
            ["DecimalSeparator"] = ("Dot", "Uses dot (.) as decimal separator"),
            ["UpdateTime"] = ("13:00", "Daily update time (around 13:00 EET)"),
            ["TimeZone"] = ("EET", "Eastern European Time"),
            ["XmlNamespace"] = ("http://www.bnr.ro/xsd", "BNR XML namespace"),
            ["HasVariableAmounts"] = ("true", "Some rates use variable amounts"),
            ["XmlStructure"] = ("DataSet/Body/Cube/Rate", "Uses DataSet > Body > Cube > Rate structure"),
            ["HasHeader"] = ("true", "Contains header with date information"),
            ["HistoricalUrl"] = ("https://www.bnr.ro/files/xml/years/nbrfxrates{year}.xml", "URL pattern for historical data by year")
        });

        _logger.LogInformation("Exchange rate providers seeded successfully.");
    }

    private async Task SeedProviderAsync(
        ExchangeRateProvider provider,
        Dictionary<string, (string Value, string Description)> configurations)
    {
        var existingProvider = await _context.ExchangeRateProviders
            .FirstOrDefaultAsync(p => p.Code == provider.Code);

        if (existingProvider == null)
        {
            _context.ExchangeRateProviders.Add(provider);
            await _context.SaveChangesAsync();

            _logger.LogInformation("  ✓ {ProviderCode} provider created (ID: {ProviderId})", provider.Code, provider.Id);

            // Add configurations
            var now = DateTimeOffset.UtcNow;
            foreach (var (key, (value, description)) in configurations)
            {
                var config = new ExchangeRateProviderConfiguration
                {
                    ProviderId = provider.Id,
                    SettingKey = key,
                    SettingValue = value,
                    Description = description,
                    Created = now
                };
                _context.ExchangeRateProviderConfigurations.Add(config);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("    ✓ {ConfigCount} configurations added", configurations.Count);
        }
        else
        {
            _logger.LogInformation("  ⚠ {ProviderCode} provider already exists (ID: {ProviderId})", provider.Code, existingProvider.Id);
        }
    }

    /// <summary>
    /// Creates database views for in-memory database.
    /// Based on views from Database/View directory.
    /// </summary>
    private async Task CreateViewsAsync()
    {
        _logger.LogInformation("Creating database views...");

        // Drop views if they exist (for re-seeding scenarios)
        await ExecuteSqlAsync("DROP VIEW IF EXISTS vw_SystemHealthDashboard");
        await ExecuteSqlAsync("DROP VIEW IF EXISTS vw_ErrorSummary");
        await ExecuteSqlAsync("DROP VIEW IF EXISTS vw_RecentFetchActivity");
        await ExecuteSqlAsync("DROP VIEW IF EXISTS vw_ProviderHealthStatus");

        // Create vw_SystemHealthDashboard
        await ExecuteSqlAsync(@"
            CREATE VIEW vw_SystemHealthDashboard AS
            SELECT
                (SELECT COUNT(*) FROM ExchangeRateProvider) AS TotalProviders,
                (SELECT COUNT(*) FROM ExchangeRateProvider WHERE IsActive = 1) AS ActiveProviders,
                (SELECT COUNT(*) FROM ExchangeRateProvider WHERE IsActive = 0) AS QuarantinedProviders,
                (SELECT COUNT(*) FROM Currency) AS TotalCurrencies,
                (SELECT COUNT(*) FROM ExchangeRate) AS TotalExchangeRates,
                (SELECT MAX(Date) FROM ExchangeRate) AS LatestRateDate,
                (SELECT MIN(Date) FROM ExchangeRate) AS OldestRateDate,
                (SELECT COUNT(*) FROM ExchangeRateFetchLog WHERE date(FetchedAt) = date('now')) AS TotalFetchesToday,
                (SELECT COUNT(*) FROM ExchangeRateFetchLog WHERE date(FetchedAt) = date('now') AND IsSuccess = 1) AS SuccessfulFetchesToday,
                (SELECT COUNT(*) FROM ExchangeRateFetchLog WHERE date(FetchedAt) = date('now') AND IsSuccess = 0) AS FailedFetchesToday,
                CAST((SELECT COUNT(*) FROM ExchangeRateFetchLog WHERE date(FetchedAt) = date('now') AND IsSuccess = 1) AS REAL) * 100.0 /
                    NULLIF((SELECT COUNT(*) FROM ExchangeRateFetchLog WHERE date(FetchedAt) = date('now')), 0) AS SuccessRateToday,
                datetime('now') AS LastUpdated
        ");
        _logger.LogInformation("  ✓ vw_SystemHealthDashboard created");

        // Create vw_ErrorSummary
        await ExecuteSqlAsync(@"
            CREATE VIEW vw_ErrorSummary AS
            SELECT
                el.Id,
                el.Severity,
                el.Source,
                el.Message,
                el.OccurredAt,
                p.Code AS ProviderCode,
                p.Name AS ProviderName
            FROM ErrorLog el
            LEFT JOIN ExchangeRateProvider p ON el.ProviderId = p.Id
        ");
        _logger.LogInformation("  ✓ vw_ErrorSummary created");

        // Create vw_RecentFetchActivity
        await ExecuteSqlAsync(@"
            CREATE VIEW vw_RecentFetchActivity AS
            SELECT
                fl.Id,
                fl.FetchedAt,
                fl.IsSuccess,
                fl.HttpStatusCode,
                fl.ResponseTimeMs,
                fl.RatesFetched,
                fl.ErrorMessage,
                p.Code AS ProviderCode,
                p.Name AS ProviderName
            FROM ExchangeRateFetchLog fl
            INNER JOIN ExchangeRateProvider p ON fl.ProviderId = p.Id
        ");
        _logger.LogInformation("  ✓ vw_RecentFetchActivity created");

        // Create vw_ProviderHealthStatus
        await ExecuteSqlAsync(@"
            CREATE VIEW vw_ProviderHealthStatus AS
            SELECT
                p.Id AS ProviderId,
                p.Code AS ProviderCode,
                p.Name AS ProviderName,
                p.IsActive,
                p.ConsecutiveFailures,
                p.LastSuccessfulFetch,
                p.LastFailedFetch,
                (SELECT COUNT(*) FROM ExchangeRateFetchLog fl WHERE fl.ProviderId = p.Id) AS TotalFetchAttempts,
                (SELECT COUNT(*) FROM ExchangeRateFetchLog fl WHERE fl.ProviderId = p.Id AND fl.IsSuccess = 1) AS SuccessfulFetches,
                (SELECT COUNT(*) FROM ExchangeRateFetchLog fl WHERE fl.ProviderId = p.Id AND fl.IsSuccess = 0) AS FailedFetches,
                (SELECT SUM(fl.RatesFetched) FROM ExchangeRateFetchLog fl WHERE fl.ProviderId = p.Id AND fl.IsSuccess = 1) AS TotalRatesProvided,
                (SELECT MIN(fl.FetchedAt) FROM ExchangeRateFetchLog fl WHERE fl.ProviderId = p.Id) AS FirstFetchDate,
                (SELECT MAX(fl.FetchedAt) FROM ExchangeRateFetchLog fl WHERE fl.ProviderId = p.Id) AS LastFetchDate,
                CAST((SELECT COUNT(*) FROM ExchangeRateFetchLog fl WHERE fl.ProviderId = p.Id AND fl.IsSuccess = 1) AS REAL) * 100.0 /
                    NULLIF((SELECT COUNT(*) FROM ExchangeRateFetchLog fl WHERE fl.ProviderId = p.Id), 0) AS SuccessRate
            FROM ExchangeRateProvider p
        ");
        _logger.LogInformation("  ✓ vw_ProviderHealthStatus created");

        _logger.LogInformation("Database views created successfully.");
    }

    private async Task ExecuteSqlAsync(string sql)
    {
        await _context.Database.ExecuteSqlRawAsync(sql);
    }
}
