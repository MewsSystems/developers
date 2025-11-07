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

    /// <summary>
    /// Get health status for a specific provider.
    /// </summary>
    [OperationContract]
    Task<GetProviderHealthResponse> GetProviderHealthAsync(GetProviderHealthRequest request);

    /// <summary>
    /// Get statistics for a specific provider.
    /// </summary>
    [OperationContract]
    Task<GetProviderStatisticsResponse> GetProviderStatisticsAsync(GetProviderStatisticsRequest request);
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

/// <summary>
/// Request for getting provider health status.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetProviderHealthRequest
{
    [System.Runtime.Serialization.DataMember]
    public int ProviderId { get; set; }
}

/// <summary>
/// Response containing provider health status.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetProviderHealthResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ProviderHealthSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting provider statistics.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetProviderStatisticsRequest
{
    [System.Runtime.Serialization.DataMember]
    public int ProviderId { get; set; }
}

/// <summary>
/// Response containing provider statistics.
/// </summary>
[System.Runtime.Serialization.DataContract]
public class GetProviderStatisticsResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ProviderStatisticsSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}
