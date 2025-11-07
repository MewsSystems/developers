namespace ConsoleTestApp.Models;

public class ExchangeRateData
{
    public List<ProviderRates> Providers { get; set; } = new();
    public DateTime FetchedAt { get; set; }
    public int TotalRates { get; set; }
}

public class ProviderRates
{
    public string ProviderCode { get; set; } = string.Empty;
    public string ProviderName { get; set; } = string.Empty;
    public List<BaseCurrencyRates> BaseCurrencies { get; set; } = new();
}

public class BaseCurrencyRates
{
    public string CurrencyCode { get; set; } = string.Empty;
    public List<TargetRate> TargetRates { get; set; } = new();
}

public class TargetRate
{
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int Multiplier { get; set; }
    public DateTime ValidDate { get; set; }
}

public class ApiCallMetrics
{
    public long ResponseTimeMs { get; set; }
    public int PayloadSizeBytes { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
