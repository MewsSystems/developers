using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationLayer.Queries.SystemHealth.GetSystemHealth;
using ApplicationLayer.Queries.SystemHealth.GetRecentErrors;
using ApplicationLayer.Queries.SystemHealth.GetFetchActivity;
using REST.Response.Models.Common;
using REST.Response.Models.Areas.SystemHealth;
using REST.Response.Converters;

namespace REST.Areas.SystemHealth;

/// <summary>
/// API endpoints for system health monitoring.
/// </summary>
[ApiController]
[Area("SystemHealth")]
[Route("api/system-health")]
[Authorize(Roles = "Admin")]
public class SystemHealthController : ControllerBase
{
    private readonly IMediator _mediator;

    public SystemHealthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get overall system health status.
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Consumer,Admin")]
    [ProducesResponseType(typeof(ApiResponse<HealthCheckResponse>), 200)]
    public async Task<IActionResult> GetHealth()
    {
        var query = new GetSystemHealthQuery();
        var healthDto = await _mediator.Send(query);

        var response = ApiResponse<HealthCheckResponse>.Ok(
            healthDto.ToResponse(),
            "System health retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get recent system errors.
    /// </summary>
    /// <param name="count">Number of errors to retrieve (default: 50)</param>
    /// <param name="severity">Filter by severity level</param>
    [HttpGet("errors")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<ErrorSummaryResponse>>), 200)]
    public async Task<IActionResult> GetRecentErrors(
        [FromQuery] int count = 50,
        [FromQuery] string? severity = null)
    {
        var query = new GetRecentErrorsQuery(count, severity);
        var pagedResult = await _mediator.Send(query);

        var response = ApiResponse<IEnumerable<ErrorSummaryResponse>>.Ok(
            pagedResult.Items.Select(e => e.ToResponse()),
            $"Recent {count} errors retrieved successfully"
        );

        return Ok(response);
    }

    /// <summary>
    /// Get recent fetch activity logs.
    /// </summary>
    /// <param name="count">Number of activity logs to retrieve (default: 50)</param>
    /// <param name="providerId">Filter by provider ID</param>
    /// <param name="failedOnly">Show only failed fetches</param>
    [HttpGet("fetch-activity")]
    [Authorize(Roles = "Consumer,Admin")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<FetchActivityResponse>>), 200)]
    public async Task<IActionResult> GetFetchActivity(
        [FromQuery] int count = 50,
        [FromQuery] int? providerId = null,
        [FromQuery] bool failedOnly = false)
    {
        var query = new GetFetchActivityQuery(count, providerId, failedOnly);
        var pagedResult = await _mediator.Send(query);

        var response = ApiResponse<IEnumerable<FetchActivityResponse>>.Ok(
            pagedResult.Items.Select(f => f.ToResponse()),
            $"Recent {count} fetch activity logs retrieved successfully"
        );

        return Ok(response);
    }
}
