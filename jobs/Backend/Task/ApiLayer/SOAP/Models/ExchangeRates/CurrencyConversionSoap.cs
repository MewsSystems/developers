using System.Runtime.Serialization;
using SOAP.Models.Common;

namespace SOAP.Models.ExchangeRates;

/// <summary>
/// SOAP model for currency conversion result.
/// </summary>
[DataContract(Namespace = "")]
public class CurrencyConversionSoap
{
    [DataMember]
    public decimal SourceAmount { get; set; }

    [DataMember]
    public decimal TargetAmount { get; set; }

    [DataMember]
    public string SourceCurrency { get; set; } = string.Empty;

    [DataMember]
    public string TargetCurrency { get; set; } = string.Empty;

    [DataMember]
    public RateInfoSoap RateInfo { get; set; } = new();

    [DataMember]
    public string ValidDate { get; set; } = string.Empty;

    [DataMember]
    public ProviderInfoSoap Provider { get; set; } = new();
}
