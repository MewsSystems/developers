using System.Runtime.Serialization;

namespace SOAP.Models.SystemHealth;

/// <summary>
/// SOAP data model for fetch activity information.
/// </summary>
[DataContract(Namespace = "")]
public class FetchActivitySoap
{
    [DataMember]
    public long LogId { get; set; }

    [DataMember]
    public int ProviderId { get; set; }

    [DataMember]
    public string ProviderCode { get; set; } = string.Empty;

    [DataMember]
    public string ProviderName { get; set; } = string.Empty;

    [DataMember]
    public string Status { get; set; } = string.Empty;

    [DataMember]
    public int? RatesImported { get; set; }

    [DataMember]
    public int? RatesUpdated { get; set; }

    [DataMember]
    public string? ErrorMessage { get; set; }

    [DataMember]
    public DateTimeOffset StartedAt { get; set; }

    [DataMember]
    public DateTimeOffset? CompletedAt { get; set; }

    [DataMember]
    public string? Duration { get; set; }
}
