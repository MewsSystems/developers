using System.ServiceModel;
using SOAP.Models.Providers;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for provider operations.
/// </summary>
[ServiceContract]
public interface IProviderService
{
    /// <summary>
    /// Get all providers.
    /// </summary>
    [OperationContract]
    Task<GetAllProvidersResponse> GetAllProvidersAsync(GetAllProvidersRequest request);

    /// <summary>
    /// Get a provider by ID.
    /// </summary>
    [OperationContract]
    Task<GetProviderByIdResponse> GetProviderByIdAsync(GetProviderByIdRequest request);
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for getting all providers.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetAllProvidersRequest
{
    // Empty request - retrieves all providers
}

/// <summary>
/// Response containing all providers.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetAllProvidersResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ProviderSoap[] Data { get; set; } = Array.Empty<ProviderSoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting a provider by ID.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetProviderByIdRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Id { get; set; }
}

/// <summary>
/// Response containing a single provider.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetProviderByIdResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ProviderSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}
