using System.Runtime.Serialization;
using SOAP.Models.Common;

namespace SOAP.Models.ExchangeRates;

/// <summary>
/// SOAP response for grouped current exchange rates.
/// Structure: Provider → Base Currencies → Target Currencies
/// </summary>
[DataContract(Namespace = "")]
public class CurrentExchangeRatesGroupedSoap
{
    [DataMember]
    public ProviderInfoSoap Provider { get; set; } = new();

    [DataMember]
    public CurrentBaseCurrencyGroupSoap[] BaseCurrencies { get; set; } = Array.Empty<CurrentBaseCurrencyGroupSoap>();

    [DataMember]
    public int TotalBaseCurrencies { get; set; }

    [DataMember]
    public int TotalRates { get; set; }
}

[DataContract(Namespace = "")]
public class CurrentBaseCurrencyGroupSoap
{
    [DataMember]
    public string BaseCurrency { get; set; } = string.Empty;

    [DataMember]
    public CurrentTargetCurrencyRateSoap[] Rates { get; set; } = Array.Empty<CurrentTargetCurrencyRateSoap>();

    [DataMember]
    public int TotalTargetCurrencies { get; set; }
}

[DataContract(Namespace = "")]
public class CurrentTargetCurrencyRateSoap
{
    [DataMember]
    public string TargetCurrency { get; set; } = string.Empty;

    [DataMember]
    public RateInfoSoap RateInfo { get; set; } = new();

    [DataMember]
    public string ValidDate { get; set; } = string.Empty;

    [DataMember]
    public string LastUpdated { get; set; } = string.Empty;

    [DataMember]
    public int DaysOld { get; set; }
}
