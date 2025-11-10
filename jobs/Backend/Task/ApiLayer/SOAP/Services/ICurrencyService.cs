using System.ServiceModel;
using SOAP.Models.Currencies;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for currency operations.
/// </summary>
[ServiceContract(Namespace = "")]
public interface ICurrencyService
{
    /// <summary>
    /// Get all currencies.
    /// </summary>
    [OperationContract]
    Task<GetAllCurrenciesResponse> GetAllCurrenciesAsync(GetAllCurrenciesRequest request);

    /// <summary>
    /// Get a currency by ID.
    /// </summary>
    [OperationContract]
    Task<GetCurrencyByIdResponse> GetCurrencyByIdAsync(GetCurrencyByIdRequest request);

    /// <summary>
    /// Get a currency by code.
    /// </summary>
    [OperationContract]
    Task<GetCurrencyByCodeResponse> GetCurrencyByCodeAsync(GetCurrencyByCodeRequest request);

    /// <summary>
    /// Create a new currency (Admin only).
    /// </summary>
    [OperationContract]
    Task<CreateCurrencyResponse> CreateCurrencyAsync(CreateCurrencyRequest request);

    /// <summary>
    /// Delete a currency (Admin only).
    /// </summary>
    [OperationContract]
    Task<DeleteCurrencyResponse> DeleteCurrencyAsync(DeleteCurrencyRequest request);
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for getting all currencies.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllCurrenciesRequest
{
    // Empty request - retrieves all currencies
}

/// <summary>
/// Response containing all currencies.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllCurrenciesResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public CurrencySoap[] Data { get; set; } = Array.Empty<CurrencySoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting a currency by ID.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetCurrencyByIdRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Id { get; set; }
}

/// <summary>
/// Response containing a single currency (by ID).
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetCurrencyByIdResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public CurrencySoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting a currency by code.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetCurrencyByCodeRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response containing a single currency (by code).
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetCurrencyByCodeResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public CurrencySoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for creating a currency.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class CreateCurrencyRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response for currency creation.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class CreateCurrencyResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public CurrencySoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for deleting a currency.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class DeleteCurrencyRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response for currency deletion.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class DeleteCurrencyResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}
