using ApplicationLayer.Queries.Providers.GetAllProviders;
using ApplicationLayer.Queries.Providers.GetProviderById;
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
}
