using System.ServiceModel;
using SOAP.Models.Providers;

namespace SOAP.Services;

/// <summary>
/// SOAP service contract for provider operations.
/// </summary>
[ServiceContract(Namespace = "")]
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

    /// <summary>
    /// Get a provider by code.
    /// </summary>
    [OperationContract]
    Task<GetProviderByCodeResponse> GetProviderByCodeAsync(GetProviderByCodeRequest request);

    /// <summary>
    /// Get provider configuration (Admin only).
    /// </summary>
    [OperationContract]
    Task<GetProviderConfigurationResponse> GetProviderConfigurationAsync(GetProviderConfigurationRequest request);

    /// <summary>
    /// Activate a provider (Admin only).
    /// </summary>
    [OperationContract]
    Task<ActivateProviderResponse> ActivateProviderAsync(ActivateProviderRequest request);

    /// <summary>
    /// Deactivate a provider (Admin only).
    /// </summary>
    [OperationContract]
    Task<DeactivateProviderResponse> DeactivateProviderAsync(DeactivateProviderRequest request);

    /// <summary>
    /// Reset provider health status (Admin only).
    /// </summary>
    [OperationContract]
    Task<ResetProviderHealthResponse> ResetProviderHealthAsync(ResetProviderHealthRequest request);

    /// <summary>
    /// Trigger manual fetch for a provider (Admin only).
    /// </summary>
    [OperationContract]
    Task<TriggerManualFetchResponse> TriggerManualFetchAsync(TriggerManualFetchRequest request);

    /// <summary>
    /// Create a new provider (Admin only).
    /// </summary>
    [OperationContract]
    Task<CreateProviderResponse> CreateProviderAsync(CreateProviderRequest request);

    /// <summary>
    /// Update provider configuration (Admin only).
    /// </summary>
    [OperationContract]
    Task<UpdateProviderConfigurationResponse> UpdateProviderConfigurationAsync(UpdateProviderConfigurationRequest request);

    /// <summary>
    /// Delete a provider (Admin only).
    /// </summary>
    [OperationContract]
    Task<DeleteProviderResponse> DeleteProviderAsync(DeleteProviderRequest request);

    /// <summary>
    /// Reschedule a provider's job (Admin only).
    /// </summary>
    [OperationContract]
    Task<RescheduleProviderResponse> RescheduleProviderAsync(RescheduleProviderRequest request);
}

// ============================================================
// REQUEST/RESPONSE MODELS
// ============================================================

/// <summary>
/// Request for getting all providers.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetAllProvidersRequest
{
    // Empty request - retrieves all providers
}

/// <summary>
/// Response containing all providers.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
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
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetProviderByIdRequest
{
    [System.Runtime.Serialization.DataMember]
    public int Id { get; set; }
}

/// <summary>
/// Response containing a single provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
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
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetProviderHealthRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response containing provider health status.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
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
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetProviderStatisticsRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response containing provider statistics.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
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

/// <summary>
/// Request for rescheduling a provider's job.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class RescheduleProviderRequest
{
    [System.Runtime.Serialization.DataMember]
    public string ProviderCode { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string UpdateTime { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string TimeZone { get; set; } = string.Empty;
}

/// <summary>
/// Response for rescheduling a provider's job.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class RescheduleProviderResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for getting a provider by code.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetProviderByCodeRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response containing a provider by code.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetProviderByCodeResponse
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
/// Request for getting provider configuration.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetProviderConfigurationRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response containing provider configuration.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class GetProviderConfigurationResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public ProviderDetailSoap? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for activating a provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ActivateProviderRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response for activating a provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ActivateProviderResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for deactivating a provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class DeactivateProviderRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response for deactivating a provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class DeactivateProviderResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for resetting provider health.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ResetProviderHealthRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response for resetting provider health.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class ResetProviderHealthResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for triggering manual fetch.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class TriggerManualFetchRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;
}

/// <summary>
/// Response for triggering manual fetch.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class TriggerManualFetchResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Data { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for creating a provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class CreateProviderRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Name { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Url { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public int BaseCurrencyId { get; set; }

    [System.Runtime.Serialization.DataMember]
    public bool RequiresAuthentication { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? ApiKeyVaultReference { get; set; }
}

/// <summary>
/// Response for creating a provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class CreateProviderResponse
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
/// Request for updating provider configuration.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class UpdateProviderConfigurationRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Name { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public string Url { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public bool RequiresAuthentication { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? ApiKeyVaultReference { get; set; }
}

/// <summary>
/// Response for updating provider configuration.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class UpdateProviderConfigurationResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}

/// <summary>
/// Request for deleting a provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class DeleteProviderRequest
{
    [System.Runtime.Serialization.DataMember]
    public string Code { get; set; } = string.Empty;

    [System.Runtime.Serialization.DataMember]
    public bool Force { get; set; }
}

/// <summary>
/// Response for deleting a provider.
/// </summary>
[System.Runtime.Serialization.DataContract(Namespace = "")]
public class DeleteProviderResponse
{
    [System.Runtime.Serialization.DataMember]
    public bool Success { get; set; }

    [System.Runtime.Serialization.DataMember]
    public string? Message { get; set; }

    [System.Runtime.Serialization.DataMember]
    public SoapFault? Fault { get; set; }
}
