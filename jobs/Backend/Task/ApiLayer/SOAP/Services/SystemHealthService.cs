using ApplicationLayer.Queries.SystemHealth.GetFetchActivity;
using ApplicationLayer.Queries.SystemHealth.GetRecentErrors;
using ApplicationLayer.Queries.SystemHealth.GetSystemHealth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SOAP.Converters;

namespace SOAP.Services;

/// <summary>
/// SOAP service implementation for system health operations.
/// Reuses ApplicationLayer queries via MediatR.
/// </summary>
public class SystemHealthService : ISystemHealthService
{
    private readonly IMediator _mediator;
    private readonly ILogger<SystemHealthService> _logger;

    public SystemHealthService(
        IMediator mediator,
        ILogger<SystemHealthService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [Authorize(Roles = "Consumer,Admin")]
    public async Task<GetSystemHealthResponse> GetSystemHealthAsync(GetSystemHealthRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetSystemHealth called");

            var query = new GetSystemHealthQuery();
            var healthDto = await _mediator.Send(query);

            return new GetSystemHealthResponse
            {
                Success = true,
                Message = "System health retrieved successfully",
                Data = healthDto.ToSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetSystemHealth SOAP operation");

            return new GetSystemHealthResponse
            {
                Success = false,
                Message = "An error occurred while retrieving system health",
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
    public async Task<GetRecentErrorsResponse> GetRecentErrorsAsync(GetRecentErrorsRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetRecentErrors called with Count: {Count}, Severity: {Severity}",
                request.Count, request.Severity);

            var query = new GetRecentErrorsQuery(request.Count, request.Severity);
            var pagedResult = await _mediator.Send(query);

            return new GetRecentErrorsResponse
            {
                Success = true,
                Message = $"Recent {request.Count} errors retrieved successfully",
                Data = pagedResult.Items.ToSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetRecentErrors SOAP operation");

            return new GetRecentErrorsResponse
            {
                Success = false,
                Message = "An error occurred while retrieving recent errors",
                Data = Array.Empty<Models.SystemHealth.ErrorSummarySoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    [Authorize(Roles = "Consumer,Admin")]
    public async Task<GetFetchActivityResponse> GetFetchActivityAsync(GetFetchActivityRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetFetchActivity called with Count: {Count}, ProviderId: {ProviderId}, FailedOnly: {FailedOnly}",
                request.Count, request.ProviderId, request.FailedOnly);

            var query = new GetFetchActivityQuery(request.Count, request.ProviderId, request.FailedOnly);
            var pagedResult = await _mediator.Send(query);

            return new GetFetchActivityResponse
            {
                Success = true,
                Message = $"Recent {request.Count} fetch activity logs retrieved successfully",
                Data = pagedResult.Items.ToSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetFetchActivity SOAP operation");

            return new GetFetchActivityResponse
            {
                Success = false,
                Message = "An error occurred while retrieving fetch activity",
                Data = Array.Empty<Models.SystemHealth.FetchActivitySoap>(),
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
