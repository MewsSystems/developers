using ApplicationLayer.Queries.Providers.GetAllProviders;
using ApplicationLayer.Queries.Providers.GetProviderById;
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
            var query = new GetAllProvidersQuery(PageNumber: 1, PageSize: 1000);
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
            _logger.LogInformation("SOAP: GetProviderHealth called for Provider ID: {ProviderId}", request.ProviderId);

            // Reuse existing ApplicationLayer query
            var query = new GetProviderHealthQuery(request.ProviderId);
            var healthDto = await _mediator.Send(query);

            if (healthDto != null)
            {
                return new GetProviderHealthResponse
                {
                    Success = true,
                    Message = $"Health status for provider {request.ProviderId} retrieved successfully",
                    Data = healthDto.ToSoap()
                };
            }

            return new GetProviderHealthResponse
            {
                Success = false,
                Message = $"Health status for provider {request.ProviderId} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Health status for provider with ID {request.ProviderId} not found"
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
            _logger.LogInformation("SOAP: GetProviderStatistics called for Provider ID: {ProviderId}", request.ProviderId);

            // Reuse existing ApplicationLayer query
            var query = new GetProviderStatisticsQuery(request.ProviderId);
            var statsDto = await _mediator.Send(query);

            if (statsDto != null)
            {
                return new GetProviderStatisticsResponse
                {
                    Success = true,
                    Message = $"Statistics for provider {request.ProviderId} retrieved successfully",
                    Data = statsDto.ToSoap()
                };
            }

            return new GetProviderStatisticsResponse
            {
                Success = false,
                Message = $"Statistics for provider {request.ProviderId} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Statistics for provider with ID {request.ProviderId} not found"
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
}
