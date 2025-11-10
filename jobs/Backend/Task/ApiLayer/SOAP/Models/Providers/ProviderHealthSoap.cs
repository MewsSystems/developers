using System.Runtime.Serialization;

namespace SOAP.Models.Providers;

/// <summary>
/// SOAP model for provider health status.
/// </summary>
[DataContract(Namespace = "")]
public class ProviderHealthSoap
{
    [DataMember]
    public int ProviderId { get; set; }

    [DataMember]
    public string ProviderCode { get; set; } = string.Empty;

    [DataMember]
    public string ProviderName { get; set; } = string.Empty;

    [DataMember]
    public string Status { get; set; } = string.Empty;

    [DataMember]
    public bool IsHealthy { get; set; }

    [DataMember]
    public int ConsecutiveFailures { get; set; }

    [DataMember]
    public string? LastSuccessfulFetch { get; set; }

    [DataMember]
    public string? LastFailedFetch { get; set; }

    [DataMember]
    public string? TimeSinceLastSuccess { get; set; }

    [DataMember]
    public string? TimeSinceLastFailure { get; set; }
}
