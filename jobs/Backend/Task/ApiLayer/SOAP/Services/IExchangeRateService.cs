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
