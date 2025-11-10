using System.Runtime.Serialization;

namespace SOAP.Models.Common;

/// <summary>
/// SOAP model for provider information.
/// </summary>
[DataContract(Namespace = "")]
public class ProviderInfoSoap
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Code { get; set; } = string.Empty;

    [DataMember]
    public string Name { get; set; } = string.Empty;
}
