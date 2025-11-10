using System.Runtime.Serialization;

namespace SOAP.Models.SystemHealth;

/// <summary>
/// SOAP data model for error summary information.
/// </summary>
[DataContract(Namespace = "")]
public class ErrorSummarySoap
{
    [DataMember]
    public string ErrorType { get; set; } = string.Empty;

    [DataMember]
    public string ErrorMessage { get; set; } = string.Empty;

    [DataMember]
    public int OccurrenceCount { get; set; }

    [DataMember]
    public DateTimeOffset FirstOccurrence { get; set; }

    [DataMember]
    public DateTimeOffset LastOccurrence { get; set; }

    [DataMember]
    public string[] AffectedProviders { get; set; } = Array.Empty<string>();
}
