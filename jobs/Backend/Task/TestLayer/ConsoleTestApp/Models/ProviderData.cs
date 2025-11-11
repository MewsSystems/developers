namespace ConsoleTestApp.Models;

public class ProviderData
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string BaseUrl { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ProvidersListData
{
    public List<ProviderData> Providers { get; set; } = new();
    public int TotalCount { get; set; }
}

public class ProviderHealthData
{
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public bool IsHealthy { get; set; }
    public int ConsecutiveFailures { get; set; }
    public DateTime? LastSuccessfulFetch { get; set; }
    public DateTime? LastFailedFetch { get; set; }
    public string? LastError { get; set; }
}

public class ProviderStatisticsData
{
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public int TotalFetches { get; set; }
    public int SuccessfulFetches { get; set; }
    public int FailedFetches { get; set; }
    public double SuccessRate { get; set; }
    public int TotalRatesProvided { get; set; }
    public DateTime? LastFetchAt { get; set; }
}

public class ProviderConfigurationData
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Url { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int BaseCurrencyId { get; set; }
    public string? BaseCurrencyCode { get; set; }
    public bool RequiresAuthentication { get; set; }
    public string? ApiKeyVaultReference { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}
