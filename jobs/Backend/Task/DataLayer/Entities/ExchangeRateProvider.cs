namespace DataLayer.Entities;

public class ExchangeRateProvider
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int BaseCurrencyId { get; set; }
    public bool RequiresAuthentication { get; set; }
    public string? ApiKeyVaultReference { get; set; }
    public bool IsActive { get; set; }
    public DateTimeOffset? LastSuccessfulFetch { get; set; }
    public DateTimeOffset? LastFailedFetch { get; set; }
    public int ConsecutiveFailures { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Modified { get; set; }

    // Navigation properties
    public Currency BaseCurrency { get; set; } = null!;
    public ICollection<ExchangeRate> ExchangeRates { get; set; } = new List<ExchangeRate>();
    public ICollection<ExchangeRateProviderConfiguration> Configurations { get; set; } = new List<ExchangeRateProviderConfiguration>();
    public ICollection<ExchangeRateFetchLog> FetchLogs { get; set; } = new List<ExchangeRateFetchLog>();
}
