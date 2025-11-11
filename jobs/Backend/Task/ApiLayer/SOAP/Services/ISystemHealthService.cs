using System.ServiceModel;
using SOAP.Models.SystemHealth;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for system health operations.
/// </summary>
[ServiceContract(Namespace = "")]
public interface ISystemHealthService
{
    /// <summary>
    /// Get overall system health status.
    /// </summary>
    [OperationContract]
    Task<GetSystemHealthResponse> GetSystemHealthAsync(GetSystemHealthRequest request);

    /// <summary>
    /// Get recent system errors (Admin only).
    /// </summary>
    [OperationContract]
    Task<GetRecentErrorsResponse> GetRecentErrorsAsync(GetRecentErrorsRequest request);

    /// <summary>
    /// Get recent fetch activity logs.
    /// </summary>
    [OperationContract]
    Task<GetFetchActivityResponse> GetFetchActivityAsync(GetFetchActivityRequest request);
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for getting system health.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetSystemHealthRequest
{
    // Empty request - retrieves system health
}

/// <summary>
/// Response containing system health information.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetSystemHealthResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SystemHealthSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting recent errors.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetRecentErrorsRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Count { get; set; } = 50;

    [System.Runtime.Serialization.DataMember]
    public string? Severity { get; set; }
}

/// <summary>
/// Response containing recent errors.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetRecentErrorsResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ErrorSummarySoap[] Data { get; set; } = Array.Empty<ErrorSummarySoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting fetch activity.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetFetchActivityRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Count { get; set; } = 50;

    [System.Runtime.Serialization.DataMember]
    public int? ProviderId { get; set; }

    [System.Runtime.Serialization.DataMember]
    public bool FailedOnly { get; set; }
}

/// <summary>
/// Response containing fetch activity logs.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetFetchActivityResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public FetchActivitySoap[] Data { get; set; } = Array.Empty<FetchActivitySoap>();

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}
