namespace DataLayer.DTOs;

// sp_BulkUpsertExchangeRates DTOs
public class ExchangeRateInput
{
    public string CurrencyCode { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public int Multiplier { get; set; } = 1;
}

public class BulkUpsertResult
{
    public int InsertedCount { get; set; }
    public int UpdatedCount { get; set; }
    public int SkippedCount { get; set; }
    public int ProcessedCount { get; set; }
    public int TotalInJson { get; set; }
    public string Status { get; set; } = string.Empty;
}

// sp_StartFetchLog DTOs
public class StartFetchLogResult
{
    public long LogId { get; set; }
    public DateTimeOffset FetchStarted { get; set; }
    public string Status { get; set; } = string.Empty;
}

// sp_CompleteFetchLog DTOs
public class CompleteFetchLogResult
{
    public long LogId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? RatesImported { get; set; }
    public int? RatesUpdated { get; set; }
    public int ProviderId { get; set; }
    public int? ProviderConsecutiveFailures { get; set; }
    public string CompletionStatus { get; set; } = string.Empty;
}
