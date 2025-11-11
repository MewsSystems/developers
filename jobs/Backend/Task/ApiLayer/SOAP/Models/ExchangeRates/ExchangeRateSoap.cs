using System.Runtime.Serialization;

namespace SOAP.Models.ExchangeRates;

/// <summary>
/// SOAP model for a single exchange rate.
/// </summary>
[DataContract(Namespace = "")]
public class ExchangeRateSoap
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public int ProviderId { get; set; }

    [DataMember]
    public string ProviderCode { get; set; } = string.Empty;

    [DataMember]
    public string ProviderName { get; set; } = string.Empty;

    [DataMember]
    public int BaseCurrencyId { get; set; }

    [DataMember]
    public string BaseCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public int TargetCurrencyId { get; set; }

    [DataMember]
    public string TargetCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public int Multiplier { get; set; }

    [DataMember]
    public decimal Rate { get; set; }

    [DataMember]
    public decimal EffectiveRate { get; set; }

    [DataMember]
    public string ValidDate { get; set; } = string.Empty;

    [DataMember]
    public DateTimeOffset Created { get; set; }

    [DataMember]
    public DateTimeOffset? Modified { get; set; }
}
