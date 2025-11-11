using ApplicationLayer.Commands.ExchangeRateProviders.ActivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeleteProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.RescheduleProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.ResetProviderHealth;
using ApplicationLayer.Commands.ExchangeRateProviders.TriggerManualFetch;
using ApplicationLayer.Commands.ExchangeRateProviders.UpdateProviderConfiguration;
using ApplicationLayer.Queries.Providers.GetAllProviders;
using ApplicationLayer.Queries.Providers.GetProviderById;
using ApplicationLayer.Queries.Providers.GetProviderConfiguration;
using ApplicationLayer.Queries.Providers.GetProviderHealth;
using ApplicationLayer.Queries.Providers.GetProviderStatistics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SOAP.Converters;

namespace SOAP.Services;

/// <summary>
/// SOAP service implementation for provider operations.
/// Reuses ApplicationLayer queries via MediatR.
/// </summary>
[Authorize(Roles = "Consumer,Admin")]
public class ProviderService : IProviderService
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProviderService> _logger;

    public ProviderService(
        IMediator mediator,
        ILogger<ProviderService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<GetAllProvidersResponse> GetAllProvidersAsync(GetAllProvidersRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetAllProviders called");

            // Reuse existing ApplicationLayer query with large page size to get all
            var query = new GetAllProvidersQuery(PageNumber: 1, PageSize: 100);
            var pagedResult = await _mediator.Send(query);

            return new GetAllProvidersResponse
            {
                Success = true,
                Message = "Providers retrieved successfully",
                Data = pagedResult.Items.ToSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllProviders SOAP operation");

            return new GetAllProvidersResponse
            {
                Success = false,
                Message = "An error occurred while retrieving providers",
                Data = Array.Empty<Models.Providers.ProviderSoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetProviderByIdResponse> GetProviderByIdAsync(GetProviderByIdRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetProviderById called for ID: {Id}", request.Id);

            // Reuse existing ApplicationLayer query
            var query = new GetProviderByIdQuery(request.Id);
            var provider = await _mediator.Send(query);

            if (provider != null)
            {
                return new GetProviderByIdResponse
                {
                    Success = true,
                    Message = $"Provider {request.Id} retrieved successfully",
                    Data = provider.ToSoap()
                };
            }

            return new GetProviderByIdResponse
            {
                Success = false,
                Message = $"Provider with ID {request.Id} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Provider with ID {request.Id} not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProviderById SOAP operation");

            return new GetProviderByIdResponse
            {
                Success = false,
                Message = "An error occurred while retrieving provider",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetProviderHealthResponse> GetProviderHealthAsync(GetProviderHealthRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetProviderHealth called for Provider Code: {Code}", request.Code);

            // Look up provider by code to get ID
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);
            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new GetProviderHealthResponse
                {
                    Success = false,
                    Message = $"Provider with code '{request.Code}' not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code '{request.Code}' not found"
                    }
                };
            }

            // Get health status using provider ID
            var query = new GetProviderHealthQuery(provider.Id);
            var healthDto = await _mediator.Send(query);

            if (healthDto != null)
            {
                return new GetProviderHealthResponse
                {
                    Success = true,
                    Message = $"Health status for provider {request.Code} retrieved successfully",
                    Data = healthDto.ToSoap()
                };
            }

            return new GetProviderHealthResponse
            {
                Success = false,
                Message = $"Health status for provider {request.Code} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Health status for provider with code '{request.Code}' not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProviderHealth SOAP operation");

            return new GetProviderHealthResponse
            {
                Success = false,
                Message = "An error occurred while retrieving provider health status",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetProviderStatisticsResponse> GetProviderStatisticsAsync(GetProviderStatisticsRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetProviderStatistics called for Provider Code: {Code}", request.Code);

            // Look up provider by code to get ID
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);
            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new GetProviderStatisticsResponse
                {
                    Success = false,
                    Message = $"Provider with code '{request.Code}' not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code '{request.Code}' not found"
                    }
                };
            }

            // Get statistics using provider ID
            var query = new GetProviderStatisticsQuery(provider.Id);
            var statsDto = await _mediator.Send(query);

            if (statsDto != null)
            {
                return new GetProviderStatisticsResponse
                {
                    Success = true,
                    Message = $"Statistics for provider {request.Code} retrieved successfully",
                    Data = statsDto.ToSoap()
                };
            }

            return new GetProviderStatisticsResponse
            {
                Success = false,
                Message = $"Statistics for provider {request.Code} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Statistics for provider with code '{request.Code}' not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProviderStatistics SOAP operation");

            return new GetProviderStatisticsResponse
            {
                Success = false,
                Message = "An error occurred while retrieving provider statistics",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetProviderByCodeResponse> GetProviderByCodeAsync(GetProviderByCodeRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetProviderByCode called for code: {Code}", request.Code);

            // Use GetAllProviders with search term to find the provider by code
            var query = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(query);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider != null)
            {
                return new GetProviderByCodeResponse
                {
                    Success = true,
                    Message = $"Provider {request.Code} retrieved successfully",
                    Data = provider.ToSoap()
                };
            }

            return new GetProviderByCodeResponse
            {
                Success = false,
                Message = $"Provider with code {request.Code} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Provider with code {request.Code} not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProviderByCode SOAP operation");

            return new GetProviderByCodeResponse
            {
                Success = false,
                Message = "An error occurred while retrieving provider",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<GetProviderConfigurationResponse> GetProviderConfigurationAsync(GetProviderConfigurationRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetProviderConfiguration called for code: {Code}", request.Code);

            // First get provider ID from code
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new GetProviderConfigurationResponse
                {
                    Success = false,
                    Message = $"Provider with code {request.Code} not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code {request.Code} not found"
                    }
                };
            }

            var query = new GetProviderConfigurationQuery(provider.Id);
            var detailDto = await _mediator.Send(query);

            if (detailDto != null)
            {
                return new GetProviderConfigurationResponse
                {
                    Success = true,
                    Message = $"Configuration for provider {request.Code} retrieved successfully",
                    Data = detailDto.ToDetailSoap()
                };
            }

            return new GetProviderConfigurationResponse
            {
                Success = false,
                Message = $"Configuration for provider {request.Code} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Configuration for provider {request.Code} not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProviderConfiguration SOAP operation");

            return new GetProviderConfigurationResponse
            {
                Success = false,
                Message = "An error occurred while retrieving provider configuration",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<ActivateProviderResponse> ActivateProviderAsync(ActivateProviderRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: ActivateProvider called for code: {Code}", request.Code);

            // First get provider ID from code
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new ActivateProviderResponse
                {
                    Success = false,
                    Message = $"Provider with code {request.Code} not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code {request.Code} not found"
                    }
                };
            }

            var command = new ActivateProviderCommand(provider.Id);
            var result = await _mediator.Send(command);

            return new ActivateProviderResponse
            {
                Success = result.IsSuccess,
                Message = result.IsSuccess ? $"Provider {request.Code} activated successfully" : result.Error,
                Fault = result.IsSuccess ? null : new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ActivationError",
                    Detail = result.Error ?? "Failed to activate provider"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ActivateProvider SOAP operation");

            return new ActivateProviderResponse
            {
                Success = false,
                Message = "An error occurred while activating the provider",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<DeactivateProviderResponse> DeactivateProviderAsync(DeactivateProviderRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: DeactivateProvider called for code: {Code}", request.Code);

            // First get provider ID from code
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new DeactivateProviderResponse
                {
                    Success = false,
                    Message = $"Provider with code {request.Code} not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code {request.Code} not found"
                    }
                };
            }

            var command = new DeactivateProviderCommand(provider.Id);
            var result = await _mediator.Send(command);

            return new DeactivateProviderResponse
            {
                Success = result.IsSuccess,
                Message = result.IsSuccess ? $"Provider {request.Code} deactivated successfully" : result.Error,
                Fault = result.IsSuccess ? null : new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "DeactivationError",
                    Detail = result.Error ?? "Failed to deactivate provider"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeactivateProvider SOAP operation");

            return new DeactivateProviderResponse
            {
                Success = false,
                Message = "An error occurred while deactivating the provider",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<ResetProviderHealthResponse> ResetProviderHealthAsync(ResetProviderHealthRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: ResetProviderHealth called for code: {Code}", request.Code);

            // First get provider ID from code
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new ResetProviderHealthResponse
                {
                    Success = false,
                    Message = $"Provider with code {request.Code} not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code {request.Code} not found"
                    }
                };
            }

            var command = new ResetProviderHealthCommand(provider.Id);
            var result = await _mediator.Send(command);

            return new ResetProviderHealthResponse
            {
                Success = result.IsSuccess,
                Message = result.IsSuccess ? $"Health status for provider {request.Code} reset successfully" : result.Error,
                Fault = result.IsSuccess ? null : new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ResetError",
                    Detail = result.Error ?? "Failed to reset provider health"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ResetProviderHealth SOAP operation");

            return new ResetProviderHealthResponse
            {
                Success = false,
                Message = "An error occurred while resetting provider health",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<TriggerManualFetchResponse> TriggerManualFetchAsync(TriggerManualFetchRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: TriggerManualFetch called for code: {Code}", request.Code);

            // First get provider ID from code
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new TriggerManualFetchResponse
                {
                    Success = false,
                    Message = $"Provider with code {request.Code} not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code {request.Code} not found"
                    }
                };
            }

            var command = new TriggerManualFetchCommand(provider.Id);
            var result = await _mediator.Send(command);

            return new TriggerManualFetchResponse
            {
                Success = result.IsSuccess,
                Message = result.IsSuccess ? $"Manual fetch triggered for provider {request.Code}" : result.Error,
                Data = result.Value,
                Fault = result.IsSuccess ? null : new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "FetchError",
                    Detail = result.Error ?? "Failed to trigger manual fetch"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in TriggerManualFetch SOAP operation");

            return new TriggerManualFetchResponse
            {
                Success = false,
                Message = "An error occurred while triggering manual fetch",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<CreateProviderResponse> CreateProviderAsync(CreateProviderRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: CreateProvider called for code: {Code}", request.Code);

            var command = new CreateExchangeRateProviderCommand(
                request.Name,
                request.Code,
                request.Url,
                request.BaseCurrencyId,
                request.RequiresAuthentication,
                request.ApiKeyVaultReference);

            var result = await _mediator.Send(command);

            if (result.IsSuccess && result.Value > 0)
            {
                // Query for the created provider
                var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
                var pagedResult = await _mediator.Send(providersQuery);

                var provider = pagedResult.Items.FirstOrDefault(p =>
                    p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

                if (provider != null)
                {
                    return new CreateProviderResponse
                    {
                        Success = true,
                        Message = $"Provider {request.Code} created successfully",
                        Data = provider.ToSoap()
                    };
                }
            }

            return new CreateProviderResponse
            {
                Success = false,
                Message = result.Error ?? "Failed to create provider",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "CreateError",
                    Detail = result.Error ?? "Failed to create provider"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateProvider SOAP operation");

            return new CreateProviderResponse
            {
                Success = false,
                Message = "An error occurred while creating the provider",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<UpdateProviderConfigurationResponse> UpdateProviderConfigurationAsync(UpdateProviderConfigurationRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: UpdateProviderConfiguration called for code: {Code}", request.Code);

            // First get provider ID from code
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new UpdateProviderConfigurationResponse
                {
                    Success = false,
                    Message = $"Provider with code {request.Code} not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code {request.Code} not found"
                    }
                };
            }

            var command = new UpdateProviderConfigurationCommand(
                provider.Id,
                request.Name,
                request.Url,
                request.RequiresAuthentication,
                request.ApiKeyVaultReference);

            var result = await _mediator.Send(command);

            return new UpdateProviderConfigurationResponse
            {
                Success = result.IsSuccess,
                Message = result.IsSuccess ? $"Configuration for provider {request.Code} updated successfully" : result.Error,
                Fault = result.IsSuccess ? null : new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "UpdateError",
                    Detail = result.Error ?? "Failed to update provider configuration"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateProviderConfiguration SOAP operation");

            return new UpdateProviderConfigurationResponse
            {
                Success = false,
                Message = "An error occurred while updating provider configuration",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<DeleteProviderResponse> DeleteProviderAsync(DeleteProviderRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: DeleteProvider called for code: {Code}", request.Code);

            // First get provider ID from code
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider == null)
            {
                return new DeleteProviderResponse
                {
                    Success = false,
                    Message = $"Provider with code {request.Code} not found",
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "NotFound",
                        Detail = $"Provider with code {request.Code} not found"
                    }
                };
            }

            var command = new DeleteProviderCommand(provider.Id, request.Force);
            var result = await _mediator.Send(command);

            return new DeleteProviderResponse
            {
                Success = result.IsSuccess,
                Message = result.IsSuccess ? $"Provider {request.Code} deleted successfully" : result.Error,
                Fault = result.IsSuccess ? null : new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "DeleteError",
                    Detail = result.Error ?? "Failed to delete provider"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteProvider SOAP operation");

            return new DeleteProviderResponse
            {
                Success = false,
                Message = "An error occurred while deleting the provider",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Admin")]
    public async Task<RescheduleProviderResponse> RescheduleProviderAsync(RescheduleProviderRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: RescheduleProvider called for {ProviderCode}", request.ProviderCode);

            var command = new RescheduleProviderCommand(
                request.ProviderCode,
                request.UpdateTime,
                request.TimeZone);

            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return new RescheduleProviderResponse
                {
                    Success = true,
                    Message = $"Provider {request.ProviderCode} rescheduled to {request.UpdateTime} ({request.TimeZone})"
                };
            }

            return new RescheduleProviderResponse
            {
                Success = false,
                Message = result.Error ?? "Failed to reschedule provider",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "RescheduleError",
                    Detail = result.Error ?? "Failed to reschedule provider"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in RescheduleProvider SOAP operation");

            return new RescheduleProviderResponse
            {
                Success = false,
                Message = "An error occurred while rescheduling the provider",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }
}
