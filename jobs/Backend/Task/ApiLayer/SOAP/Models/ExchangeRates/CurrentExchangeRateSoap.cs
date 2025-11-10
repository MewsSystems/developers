using System.Runtime.Serialization;

namespace SOAP.Models.ExchangeRates;

/// <summary>
/// SOAP model for current exchange rates (flat list).
/// </summary>
[DataContract(Namespace = "")]
public class CurrentExchangeRateSoap
{
    [DataMember]
    public string BaseCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public string TargetCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public decimal Rate { get; set; }

    [DataMember]
    public int Multiplier { get; set; }

    [DataMember]
    public decimal EffectiveRate { get; set; }

    [DataMember]
    public string ValidDate { get; set; } = string.Empty;

    [DataMember]
    public string ProviderCode { get; set; } = string.Empty;

    [DataMember]
    public DateTimeOffset LastUpdated { get; set; }

    [DataMember]
    public int DaysOld { get; set; }
}
