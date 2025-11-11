using System.ServiceModel;
using SOAP.Models.ExchangeRates;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for exchange rate operations.
/// </summary>
[ServiceContract(Namespace = "")]
public interface IExchangeRateService
{
    /// <summary>
    /// Get all latest exchange rates grouped by provider and base currency.
    /// Returns nested structure: Provider → Base Currencies → Target Currencies.
    /// </summary>
    [OperationContract]
    Task<GetAllLatestRatesGroupedResponse> GetAllLatestRatesGroupedAsync(GetAllLatestRatesGroupedRequest request);

    /// <summary>
    /// Get historical rates update notification data.
    /// Same as GetAllLatestRatesGrouped but semantically different for historical context.
    /// </summary>
    [OperationContract]
    Task<GetAllLatestRatesGroupedResponse> GetHistoricalRatesUpdateAsync(GetAllLatestRatesGroupedRequest request);

    /// <summary>
    /// Get current exchange rates grouped by provider and base currency.
    /// Returns nested structure: Provider → Base Currencies → Target Currencies.
    /// </summary>
    [OperationContract]
    Task<GetCurrentRatesResponse> GetCurrentRatesAsync(GetCurrentRatesRequest request);

    /// <summary>
    /// Convert an amount from one currency to another.
    /// </summary>
    [OperationContract]
    Task<ConvertCurrencyResponse> ConvertCurrencyAsync(ConvertCurrencyRequest request);

    /// <summary>
    /// Get the latest exchange rate for a currency pair from a specific provider.
    /// </summary>
    [OperationContract]
    Task<GetLatestRateResponse> GetLatestRateAsync(GetLatestRateRequest request);

    /// <summary>
    /// Get all latest exchange rates across all currency pairs (flat list).
    /// </summary>
    [OperationContract]
    Task<GetAllLatestRatesResponse> GetAllLatestRatesAsync(GetAllLatestRatesRequest request);

    /// <summary>
    /// Get current exchange rates (flat list).
    /// </summary>
    [OperationContract]
    Task<GetCurrentRatesFlatResponse> GetCurrentRatesFlatAsync(GetCurrentRatesFlatRequest request);

    /// <summary>
    /// Get historical exchange rates for a currency pair (flat list).
    /// </summary>
    [OperationContract]
    Task<GetHistoryResponse> GetHistoryAsync(GetHistoryRequest request);

    /// <summary>
    /// Get historical exchange rates for a currency pair grouped by provider and base currency.
    /// </summary>
    [OperationContract]
    Task<GetHistoryGroupedResponse> GetHistoryGroupedAsync(GetHistoryGroupedRequest request);
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for getting all latest exchange rates grouped.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllLatestRatesGroupedRequest
{
    // Empty request - no parameters needed for getting all rates
}

/// <summary>
/// Response containing grouped latest exchange rates.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllLatestRatesGroupedResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public LatestExchangeRatesGroupedSoap[] Data { get; set; } = Array.Empty<LatestExchangeRatesGroupedSoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting current exchange rates.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetCurrentRatesRequest
{
    // Empty request - no parameters needed for getting current rates
}

/// <summary>
/// Response containing grouped current exchange rates.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetCurrentRatesResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public CurrentExchangeRatesGroupedSoap[] Data { get; set; } = Array.Empty<CurrentExchangeRatesGroupedSoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for converting currency.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ConvertCurrencyRequest
{
    [System.Runtime.Serialization.DataMember]
    public string FromCurrency { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string ToCurrency { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public decimal Amount { get; set; }
}

/// <summary>
/// Response containing currency conversion result.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ConvertCurrencyResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public CurrencyConversionSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// SOAP fault for error responses.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class SoapFault
{
    [System.Runtime.Serialization.DataMember]
    public string FaultCode { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string FaultString { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string? Detail { get; set; }
}

/// <summary>
/// Request for getting the latest exchange rate for a currency pair.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetLatestRateRequest
{
    [System.Runtime.Serialization.DataMember]
    public string SourceCurrency { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string TargetCurrency { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public int? ProviderId { get; set; }
}

/// <summary>
/// Response containing the latest exchange rate.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetLatestRateResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ExchangeRateSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting all latest exchange rates (flat list).
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllLatestRatesRequest
{
    // Empty request - no parameters needed
}

/// <summary>
/// Response containing all latest exchange rates as a flat list.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllLatestRatesResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public LatestExchangeRateSoap[] Data { get; set; } = Array.Empty<LatestExchangeRateSoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting current exchange rates (flat list).
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetCurrentRatesFlatRequest
{
    // Empty request - no parameters needed
}

/// <summary>
/// Response containing current exchange rates as a flat list.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetCurrentRatesFlatResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public CurrentExchangeRateSoap[] Data { get; set; } = Array.Empty<CurrentExchangeRateSoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting exchange rate history (flat list).
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetHistoryRequest
{
    [System.Runtime.Serialization.DataMember]
    public string SourceCurrency { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string TargetCurrency { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string StartDate { get; set; } = string.Empty; // YYYY-MM-DD

    [System.Runtime.Serialization.DataMember]
    public string EndDate { get; set; } = string.Empty; // YYYY-MM-DD

    [System.Runtime.Serialization.DataMember]
    public int? ProviderId { get; set; }
}

/// <summary>
/// Response containing exchange rate history as a flat list.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetHistoryResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ExchangeRateHistorySoap[] Data { get; set; } = Array.Empty<ExchangeRateHistorySoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting exchange rate history grouped by provider and base currency.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetHistoryGroupedRequest
{
    [System.Runtime.Serialization.DataMember]
    public string SourceCurrency { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string TargetCurrency { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string StartDate { get; set; } = string.Empty; // YYYY-MM-DD

    [System.Runtime.Serialization.DataMember]
    public string EndDate { get; set; } = string.Empty; // YYYY-MM-DD

    [System.Runtime.Serialization.DataMember]
    public int? ProviderId { get; set; }
}

/// <summary>
/// Response containing exchange rate history grouped.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetHistoryGroupedResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ExchangeRateHistoryGroupedSoap[] Data { get; set; } = Array.Empty<ExchangeRateHistoryGroupedSoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}
