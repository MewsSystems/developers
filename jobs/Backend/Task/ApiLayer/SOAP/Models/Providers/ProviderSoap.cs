using System.Runtime.Serialization;

namespace SOAP.Models.Providers;

/// <summary>
/// SOAP model for provider information.
/// </summary>
[DataContract(Namespace = "")]
public class ProviderSoap
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Code { get; set; } = string.Empty;

    [DataMember]
    public string Name { get; set; } = string.Empty;

    [DataMember]
    public string Url { get; set; } = string.Empty;

    [DataMember]
    public string BaseCurrency { get; set; } = string.Empty;

    [DataMember]
    public bool IsActive { get; set; }

    [DataMember]
    public string HealthStatus { get; set; } = string.Empty;

    [DataMember]
    public int SuccessfulFetchCount { get; set; }

    [DataMember]
    public int FailedFetchCount { get; set; }

    [DataMember]
    public string? LastFetchAttempt { get; set; }

    [DataMember]
    public string? LastSuccessfulFetch { get; set; }
}
