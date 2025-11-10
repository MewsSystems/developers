using System.Runtime.Serialization;

namespace SOAP.Models.Providers;

/// <summary>
/// SOAP model for provider statistics and performance metrics.
/// </summary>
[DataContract(Namespace = "")]
public class ProviderStatisticsSoap
{
    [DataMember]
    public int ProviderId { get; set; }

    [DataMember]
    public string ProviderCode { get; set; } = string.Empty;

    [DataMember]
    public string ProviderName { get; set; } = string.Empty;

    [DataMember]
    public int TotalRatesProvided { get; set; }

    [DataMember]
    public int TotalFetchAttempts { get; set; }

    [DataMember]
    public int SuccessfulFetches { get; set; }

    [DataMember]
    public int FailedFetches { get; set; }

    [DataMember]
    public decimal SuccessRate { get; set; }

    [DataMember]
    public string? FirstFetchDate { get; set; }

    [DataMember]
    public string? LastFetchDate { get; set; }

    [DataMember]
    public string? AverageFetchInterval { get; set; }

    [DataMember]
    public string? OldestRateDate { get; set; }

    [DataMember]
    public string? NewestRateDate { get; set; }
}
