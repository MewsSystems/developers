using System.Runtime.Serialization;

namespace SOAP.Models.Common;

/// <summary>
/// SOAP model for rate information.
/// </summary>
[DataContract(Namespace = "")]
public class RateInfoSoap
{
    [DataMember]
    public decimal Rate { get; set; }

    [DataMember]
    public int Multiplier { get; set; }

    [DataMember]
    public decimal EffectiveRate { get; set; }
}
