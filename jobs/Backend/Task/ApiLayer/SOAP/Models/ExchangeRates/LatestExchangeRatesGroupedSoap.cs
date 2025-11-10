using System.Runtime.Serialization;
using SOAP.Models.Common;

namespace SOAP.Models.ExchangeRates;

/// <summary>
/// SOAP response for grouped latest exchange rates.
/// Structure: Provider → Base Currencies → Target Currencies
/// </summary>
[DataContract(Namespace = "")]
public class LatestExchangeRatesGroupedSoap
{
    [DataMember]
    public ProviderInfoSoap Provider { get; set; } = new();

    [DataMember]
    public BaseCurrencyGroupSoap[] BaseCurrencies { get; set; } = Array.Empty<BaseCurrencyGroupSoap>();

    [DataMember]
    public int TotalBaseCurrencies { get; set; }

    [DataMember]
    public int TotalRates { get; set; }
}

[DataContract(Namespace = "")]
public class BaseCurrencyGroupSoap
{
    [DataMember]
    public string BaseCurrency { get; set; } = string.Empty;

    [DataMember]
    public TargetCurrencyRateSoap[] Rates { get; set; } = Array.Empty<TargetCurrencyRateSoap>();

    [DataMember]
    public int TotalTargetCurrencies { get; set; }
}

[DataContract(Namespace = "")]
public class TargetCurrencyRateSoap
{
    [DataMember]
    public int Id { get; set; }

    [DataMember]
    public string TargetCurrency { get; set; } = string.Empty;

    [DataMember]
    public RateInfoSoap RateInfo { get; set; } = new();

    [DataMember]
    public string ValidDate { get; set; } = string.Empty;

    [DataMember]
    public string FetchedAt { get; set; } = string.Empty;

    [DataMember]
    public string UpdatedAt { get; set; } = string.Empty;
}
