using System.Runtime.Serialization;

namespace SOAP.Models.Currencies;

/// <summary>
/// SOAP model for currency information.
/// Currency is a value object with minimal properties (Id, Code).
/// </summary>
[DataContract(Namespace = "")]
public class CurrencySoap
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string Code { get; set; } = string.Empty;
}
