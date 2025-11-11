using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ApplicationLayer.Queries.Providers.GetAllProviders;
using ApplicationLayer.Queries.Providers.GetProviderById;
using ApplicationLayer.Queries.Providers.GetProviderHealth;
using ApplicationLayer.Queries.Providers.GetProviderStatistics;
using ApplicationLayer.Queries.Providers.GetProviderConfiguration;
using ApplicationLayer.Commands.ExchangeRateProviders.ActivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.ResetProviderHealth;
using ApplicationLayer.Commands.ExchangeRateProviders.TriggerManualFetch;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.UpdateProviderConfiguration;
using ApplicationLayer.Commands.ExchangeRateProviders.DeleteProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.RescheduleProvider;
using REST.Response.Models.Common;
using REST.Response.Models.Areas.Providers;
using REST.Response.Models.Areas.ExchangeRates;
using REST.Response.Converters;
using REST.Request.Models.Areas.Providers;

namespace REST.Areas.Providers;

/// <summary>
/// API endpoints for exchange rate provider management.
/// </summary>
[ApiController]
[Area("Providers")]
[Route("api/providers")]
[Authorize(Roles = "Consumer,Admin")]
public class ProvidersController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProvidersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all exchange rate providers with optional filtering and pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResponse<ProviderResponse>), 200)]
    public async Task<IActionResult> GetAll(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] bool? isActive = null,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetAllProvidersQuery(pageNumber, pageSize, isActive, searchTerm);
        var pagedResult = await _mediator.Send(query);

        var response = new PagedResponse<ProviderResponse>
        {
            Success = true,
            Data = pagedResult.Items.Select(p => p.ToResponse()),
            PageNumber = pagedResult.PageNumber,
            PageSize = pagedResult.PageSize,
            TotalCount = pagedResult.TotalCount,
            TotalPages = pagedResult.TotalPages,
            HasPrevious = pagedResult.HasPreviousPage,
            HasNext = pagedResult.HasNextPage,
            StatusCode = 200,
            Message = "Providers retrieved successfully"
        };
        return Ok(response);
    }

    /// <summary>
    /// Get a specific provider by ID.
    /// </summary>
    /// <param name="id">Provider ID</param>
    [HttpGet("id/{id}")]
    [ProducesResponseType(typeof(ApiResponse<ProviderDetailResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetById(int id)
    {
        var query = new GetProviderByIdQuery(id);
        var providerDto = await _mediator.Send(query);

        if (providerDto != null)
        {
            var response = ApiResponse<ProviderDetailResponse>.Ok(
                providerDto.ToResponse(),
                $"Provider with ID {id} retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<ProviderDetailResponse>.NotFound($"Provider with ID {id} not found"));
    }

    /// <summary>
    /// Get a specific provider by code.
    /// </summary>
    /// <param name="code">Provider code (e.g., ECB, CNB, BNR)</param>
    [HttpGet("{code}")]
    [ProducesResponseType(typeof(ApiResponse<ProviderResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetByCode(string code)
    {
        // Use GetAllProviders with search term to find the provider by code
        var query = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(query);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider != null)
        {
            var response = ApiResponse<ProviderResponse>.Ok(
                provider.ToResponse(),
                $"Provider {code} retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<ProviderResponse>.NotFound($"Provider with code '{code}' not found"));
    }

    /// <summary>
    /// Get health status for a specific provider.
    /// </summary>
    /// <param name="code">Provider code</param>
    [HttpGet("{code}/health")]
    [ProducesResponseType(typeof(ApiResponse<ProviderHealthResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetHealth(string code)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse<ProviderHealthResponse>.NotFound($"Provider with code '{code}' not found"));
        }

        var healthQuery = new GetProviderHealthQuery(provider.Id);
        var healthDto = await _mediator.Send(healthQuery);

        if (healthDto != null)
        {
            var response = ApiResponse<ProviderHealthResponse>.Ok(
                healthDto.ToResponse(),
                $"Health status for provider {code} retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<ProviderHealthResponse>.NotFound($"Health status for provider {code} not found"));
    }

    /// <summary>
    /// Get statistics for a specific provider.
    /// </summary>
    /// <param name="code">Provider code</param>
    [HttpGet("{code}/statistics")]
    [ProducesResponseType(typeof(ApiResponse<ProviderStatisticsResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetStatistics(string code)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse<ProviderStatisticsResponse>.NotFound($"Provider with code '{code}' not found"));
        }

        var statsQuery = new GetProviderStatisticsQuery(provider.Id);
        var statsDto = await _mediator.Send(statsQuery);

        if (statsDto != null)
        {
            var response = ApiResponse<ProviderStatisticsResponse>.Ok(
                statsDto.ToResponse(),
                $"Statistics for provider {code} retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<ProviderStatisticsResponse>.NotFound($"Statistics for provider {code} not found"));
    }

    /// <summary>
    /// Activate a provider.
    /// </summary>
    /// <param name="code">Provider code</param>
    [HttpPost("{code}/activate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> Activate(string code)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse.Fail($"Provider with code '{code}' not found", 404));
        }

        var command = new ActivateProviderCommand(provider.Id);
        var result = await _mediator.Send(command);

        return Ok(result.ToApiResponse($"Provider {code} activated successfully"));
    }

    /// <summary>
    /// Deactivate a provider.
    /// </summary>
    /// <param name="code">Provider code</param>
    [HttpPost("{code}/deactivate")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> Deactivate(string code)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse.Fail($"Provider with code '{code}' not found", 404));
        }

        var command = new DeactivateProviderCommand(provider.Id);
        var result = await _mediator.Send(command);

        return Ok(result.ToApiResponse($"Provider {code} deactivated successfully"));
    }

    /// <summary>
    /// Reset health status for a provider.
    /// </summary>
    /// <param name="code">Provider code</param>
    [HttpPost("{code}/reset-health")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> ResetHealth(string code)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse.Fail($"Provider with code '{code}' not found", 404));
        }

        var command = new ResetProviderHealthCommand(provider.Id);
        var result = await _mediator.Send(command);

        return Ok(result.ToApiResponse($"Health status for provider {code} reset successfully"));
    }

    /// <summary>
    /// Trigger a manual fetch for a provider.
    /// </summary>
    /// <param name="code">Provider code</param>
    [HttpPost("{code}/fetch")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<FetchResultResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> TriggerFetch(string code)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse.Fail($"Provider with code '{code}' not found", 404));
        }

        var command = new TriggerManualFetchCommand(provider.Id);
        var result = await _mediator.Send(command);

        if (result.IsSuccess && result.Value != null)
        {
            // The command returns a simple string message, so we create a basic response
            var response = ApiResponse<string>.Ok(
                result.Value,
                $"Manual fetch triggered for provider {code}"
            );
            return Ok(response);
        }

        return BadRequest(ApiResponse<string>.BadRequest(
            result.Error ?? "Failed to trigger manual fetch"
        ));
    }

    /// <summary>
    /// Create a new exchange rate provider.
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<ProviderResponse>), 201)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> CreateProvider([FromBody] CreateProviderRequest request)
    {
        var command = new CreateExchangeRateProviderCommand(
            request.Name,
            request.Code,
            request.Url,
            request.BaseCurrencyId,
            request.RequiresAuthentication,
            request.ApiKeyVaultReference
        );

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
                var response = ApiResponse<ProviderResponse>.Ok(
                    provider.ToResponse(),
                    $"Provider {request.Code} created successfully"
                );
                return CreatedAtAction(nameof(GetByCode), new { code = request.Code }, response);
            }
        }

        return BadRequest(ApiResponse<ProviderResponse>.BadRequest(
            result.Error ?? "Failed to create provider"
        ));
    }

    /// <summary>
    /// Get detailed configuration for a provider.
    /// </summary>
    /// <param name="code">Provider code</param>
    [HttpGet("{code}/configuration")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<ProviderDetailResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetConfiguration(string code)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse<ProviderDetailResponse>.NotFound($"Provider with code '{code}' not found"));
        }

        var query = new GetProviderConfigurationQuery(provider.Id);
        var detailDto = await _mediator.Send(query);

        if (detailDto != null)
        {
            var response = ApiResponse<ProviderDetailResponse>.Ok(
                detailDto.ToResponse(),
                $"Configuration for provider {code} retrieved successfully"
            );
            return Ok(response);
        }

        return NotFound(ApiResponse<ProviderDetailResponse>.NotFound($"Configuration for provider {code} not found"));
    }

    /// <summary>
    /// Update provider configuration.
    /// </summary>
    /// <param name="code">Provider code</param>
    [HttpPut("{code}/configuration")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> UpdateConfiguration(string code, [FromBody] UpdateProviderConfigurationRequest request)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse.Fail($"Provider with code '{code}' not found", 404));
        }

        var command = new UpdateProviderConfigurationCommand(
            provider.Id,
            request.Name,
            request.Url,
            request.RequiresAuthentication,
            request.ApiKeyVaultReference
        );

        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.ToApiResponse($"Configuration for provider {code} updated successfully"))
            : BadRequest(result.ToApiResponse());
    }

    /// <summary>
    /// Delete a provider.
    /// </summary>
    /// <param name="code">Provider code</param>
    /// <param name="force">Force deletion even if provider has associated exchange rates</param>
    [HttpDelete("{code}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> DeleteProvider(string code, [FromQuery] bool force = false)
    {
        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, code);
        var pagedResult = await _mediator.Send(providersQuery);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return NotFound(ApiResponse.Fail($"Provider with code '{code}' not found", 404));
        }

        var command = new DeleteProviderCommand(provider.Id, force);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.ToApiResponse($"Provider {code} deleted successfully"))
            : BadRequest(result.ToApiResponse());
    }

    /// <summary>
    /// Reschedule a provider's job with new time and timezone.
    /// </summary>
    /// <param name="code">Provider code</param>
    /// <param name="request">Reschedule request with UpdateTime and TimeZone</param>
    [HttpPost("{code}/reschedule")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> RescheduleProvider(string code, [FromBody] RescheduleProviderRequest request)
    {
        var command = new RescheduleProviderCommand(code, request.UpdateTime, request.TimeZone);
        var result = await _mediator.Send(command);

        return result.IsSuccess
            ? Ok(result.ToApiResponse($"Provider {code} rescheduled to {request.UpdateTime} ({request.TimeZone})"))
            : BadRequest(result.ToApiResponse());
    }
}
