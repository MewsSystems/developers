using System.Runtime.Serialization;

namespace SOAP.Models.SystemHealth;

/// <summary>
/// SOAP data model for system health information.
/// </summary>
[DataContract(Namespace = "")]
public class SystemHealthSoap
{
    [DataMember]
    public int TotalProviders { get; set; }

    [DataMember]
    public int ActiveProviders { get; set; }

    [DataMember]
    public int QuarantinedProviders { get; set; }

    [DataMember]
    public int TotalCurrencies { get; set; }

    [DataMember]
    public int TotalExchangeRates { get; set; }

    [DataMember]
    public string? LatestRateDate { get; set; }

    [DataMember]
    public string? OldestRateDate { get; set; }

    [DataMember]
    public int TotalFetchesToday { get; set; }

    [DataMember]
    public int SuccessfulFetchesToday { get; set; }

    [DataMember]
    public int FailedFetchesToday { get; set; }

    [DataMember]
    public decimal SuccessRateToday { get; set; }

    [DataMember]
    public DateTimeOffset LastUpdated { get; set; }
}
