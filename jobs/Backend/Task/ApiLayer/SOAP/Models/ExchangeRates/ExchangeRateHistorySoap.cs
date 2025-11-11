using System.Runtime.Serialization;

namespace SOAP.Models.ExchangeRates;

/// <summary>
/// SOAP model for exchange rate history over time.
/// </summary>
[DataContract(Namespace = "")]
public class ExchangeRateHistorySoap
{
    [DataMember]
    public int ProviderId { get; set; }

    [DataMember]
    public string ProviderCode { get; set; } = string.Empty;

    [DataMember]
    public string ProviderName { get; set; } = string.Empty;

    [DataMember]
    public string SourceCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public string BaseCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public string TargetCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public string ValidDate { get; set; } = string.Empty;

    [DataMember]
    public decimal Rate { get; set; }

    [DataMember]
    public int Multiplier { get; set; }

    [DataMember]
    public decimal EffectiveRate { get; set; }

    [DataMember]
    public decimal? ChangeFromPrevious { get; set; }

    [DataMember]
    public decimal? ChangePercentage { get; set; }
}

/// <summary>
/// SOAP model for grouped exchange rate history.
/// Structure: Provider → Base Currencies → Target Currencies with historical data
/// </summary>
[DataContract(Namespace = "")]
public class ExchangeRateHistoryGroupedSoap
{
    [DataMember]
    public int ProviderId { get; set; }

    [DataMember]
    public string ProviderCode { get; set; } = string.Empty;

    [DataMember]
    public string ProviderName { get; set; } = string.Empty;

    [DataMember]
    public HistoryBaseCurrencyGroupSoap[] BaseCurrencies { get; set; } = Array.Empty<HistoryBaseCurrencyGroupSoap>();
}

[DataContract(Namespace = "")]
public class HistoryBaseCurrencyGroupSoap
{
    [DataMember]
    public string BaseCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public HistoryTargetCurrencyRateSoap[] TargetCurrencies { get; set; } = Array.Empty<HistoryTargetCurrencyRateSoap>();
}

[DataContract(Namespace = "")]
public class HistoryTargetCurrencyRateSoap
{
    [DataMember]
    public string TargetCurrencyCode { get; set; } = string.Empty;

    [DataMember]
    public HistoryDataPointSoap[] History { get; set; } = Array.Empty<HistoryDataPointSoap>();
}

[DataContract(Namespace = "")]
public class HistoryDataPointSoap
{
    [DataMember]
    public string ValidDate { get; set; } = string.Empty;

    [DataMember]
    public decimal Rate { get; set; }

    [DataMember]
    public int Multiplier { get; set; }

    [DataMember]
    public decimal EffectiveRate { get; set; }

    [DataMember]
    public decimal? ChangeFromPrevious { get; set; }

    [DataMember]
    public decimal? ChangePercentage { get; set; }
}
