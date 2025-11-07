using System.ServiceModel;
using SOAP.Models.ExchangeRates;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for exchange rate operations.
/// </summary>
[ServiceContract]
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
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for getting all latest exchange rates grouped.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetAllLatestRatesGroupedRequest
{
    // Empty request - no parameters needed for getting all rates
}

/// <summary>
/// Response containing grouped latest exchange rates.
/// </summary>
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
public class GetCurrentRatesRequest
{
    // Empty request - no parameters needed for getting current rates
}

/// <summary>
/// Response containing grouped current exchange rates.
/// </summary>
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
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
[System.Runtime.Serialization.DataContract]
public class SoapFault
{
    [System.Runtime.Serialization.DataMember]
    public string FaultCode { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string FaultString { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string? Detail { get; set; }
}
