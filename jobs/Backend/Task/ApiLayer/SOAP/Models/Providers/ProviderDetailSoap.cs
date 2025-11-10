using System.Runtime.Serialization;

namespace SOAP.Models.Providers;

/// <summary>
/// SOAP data model for detailed provider information including configurations.
/// </summary>
[DataContract(Namespace = "")]
public class ProviderDetailSoap
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Name { get; set; } = string.Empty;

    [DataMember]
    public string Code { get; set; } = string.Empty;

    [DataMember]
    public string Url { get; set; } = string.Empty;

    [DataMember]
    public string BaseCurrency { get; set; } = string.Empty;

    [DataMember]
    public bool RequiresAuthentication { get; set; }

    [DataMember]
    public string? ApiKeyVaultReference { get; set; }

    [DataMember]
    public bool IsActive { get; set; }

    [DataMember]
    public string Status { get; set; } = string.Empty;

    [DataMember]
    public int ConsecutiveFailures { get; set; }

    [DataMember]
    public DateTimeOffset? LastSuccessfulFetch { get; set; }

    [DataMember]
    public DateTimeOffset? LastFailedFetch { get; set; }

    [DataMember]
    public DateTimeOffset Created { get; set; }

    [DataMember]
    public DateTimeOffset? Modified { get; set; }
}
