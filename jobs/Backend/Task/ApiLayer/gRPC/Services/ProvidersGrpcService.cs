using ApplicationLayer.Commands.ExchangeRateProviders.ActivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.CreateExchangeRateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeactivateProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.DeleteProvider;
using ApplicationLayer.Commands.ExchangeRateProviders.ResetProviderHealth;
using ApplicationLayer.Commands.ExchangeRateProviders.TriggerManualFetch;
using ApplicationLayer.Commands.ExchangeRateProviders.UpdateProviderConfiguration;
using ApplicationLayer.Queries.Providers.GetAllProviders;
using ApplicationLayer.Queries.Providers.GetProviderConfiguration;
using ApplicationLayer.Queries.Providers.GetProviderHealth;
using ApplicationLayer.Queries.Providers.GetProviderStatistics;
using gRPC.Mappers;
using gRPC.Protos.Providers;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace gRPC.Services;

/// <summary>
/// gRPC service for exchange rate provider management operations.
/// </summary>
[Authorize(Roles = "Consumer,Admin")]
public class ProvidersGrpcService : ProvidersService.ProvidersServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProvidersGrpcService> _logger;

    public ProvidersGrpcService(
        IMediator mediator,
        ILogger<ProvidersGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    // ============================================================
    // QUERY OPERATIONS
    // ============================================================

    public override async Task<GetAllProvidersResponse> GetAllProviders(
        GetAllProvidersRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetAllProviders request received");

        var query = new GetAllProvidersQuery(
            request.Paging?.PageNumber ?? 1,
            request.Paging?.PageSize ?? 10,
            request.HasIsActive ? request.IsActive : null,
            request.SearchTerm);

        var pagedResult = await _mediator.Send(query, context.CancellationToken);

        var response = new GetAllProvidersResponse
        {
            Paging = CommonMappers.ToProtoPaging(pagedResult),
            Message = "Providers retrieved successfully"
        };

        foreach (var provider in pagedResult.Items)
        {
            response.Providers.Add(new ProviderInfo
            {
                Id = provider.Id,
                Code = provider.Code,
                Name = provider.Name,
                Url = provider.Url,
                IsActive = provider.IsActive,
                BaseCurrencyCode = provider.BaseCurrencyCode,
                Status = provider.Status,
                ConsecutiveFailures = provider.ConsecutiveFailures,
                LastSuccessfulFetch = provider.LastSuccessfulFetch.HasValue
                    ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.LastSuccessfulFetch.Value)
                    : null,
                LastFailedFetch = provider.LastFailedFetch.HasValue
                    ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.LastFailedFetch.Value)
                    : null,
                Created = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.Created)
            });
        }

        return response;
    }

    public override async Task<GetProviderByCodeResponse> GetProviderByCode(
        GetProviderByCodeRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetProviderByCode request: {Code}", request.Code);

        // Query all providers and filter by code
        var query = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(query, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider != null)
        {
            return new GetProviderByCodeResponse
            {
                Success = true,
                Message = $"Provider {request.Code} retrieved successfully",
                Data = new ProviderInfo
                {
                    Id = provider.Id,
                    Code = provider.Code,
                    Name = provider.Name,
                    Url = provider.Url,
                    IsActive = provider.IsActive,
                    BaseCurrencyCode = provider.BaseCurrencyCode,
                    Status = provider.Status,
                    ConsecutiveFailures = provider.ConsecutiveFailures,
                    LastSuccessfulFetch = provider.LastSuccessfulFetch.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.LastSuccessfulFetch.Value)
                        : null,
                    LastFailedFetch = provider.LastFailedFetch.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.LastFailedFetch.Value)
                        : null,
                    Created = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.Created)
                }
            };
        }

        return new GetProviderByCodeResponse
        {
            Success = false,
            Message = $"Provider with code '{request.Code}' not found",
            Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
        };
    }

    public override async Task<GetProviderHealthResponse> GetProviderHealth(
        GetProviderHealthRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetProviderHealth request: {Code}", request.Code);

        // First get provider ID from code
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new GetProviderHealthResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var healthQuery = new GetProviderHealthQuery(provider.Id);
        var healthDto = await _mediator.Send(healthQuery, context.CancellationToken);

        if (healthDto != null)
        {
            return new GetProviderHealthResponse
            {
                Success = true,
                Message = $"Health status for provider {request.Code} retrieved successfully",
                Data = new ProviderHealthInfo
                {
                    ProviderId = healthDto.ProviderId,
                    ProviderCode = healthDto.ProviderCode,
                    ProviderName = healthDto.ProviderName,
                    HealthStatus = healthDto.Status,
                    ConsecutiveFailures = healthDto.ConsecutiveFailures,
                    LastSuccessfulFetch = healthDto.LastSuccessfulFetch.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(healthDto.LastSuccessfulFetch.Value)
                        : null,
                    LastFailedFetch = healthDto.LastFailedFetch.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(healthDto.LastFailedFetch.Value)
                        : null,
                    LastErrorMessage = ""
                }
            };
        }

        return new GetProviderHealthResponse
        {
            Success = false,
            Message = $"Health status for provider {request.Code} not found"
        };
    }

    public override async Task<GetProviderStatisticsResponse> GetProviderStatistics(
        GetProviderStatisticsRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetProviderStatistics request: {Code}", request.Code);

        // Get provider ID
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new GetProviderStatisticsResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var statsQuery = new GetProviderStatisticsQuery(provider.Id);
        var statsDto = await _mediator.Send(statsQuery, context.CancellationToken);

        if (statsDto != null)
        {
            return new GetProviderStatisticsResponse
            {
                Success = true,
                Message = $"Statistics for provider {request.Code} retrieved successfully",
                Data = new ProviderStatisticsInfo
                {
                    ProviderId = statsDto.ProviderId,
                    ProviderCode = statsDto.ProviderCode,
                    ProviderName = statsDto.ProviderName,
                    TotalRatesProvided = statsDto.TotalRatesProvided,
                    TotalFetchAttempts = statsDto.TotalFetchAttempts,
                    SuccessfulFetches = statsDto.SuccessfulFetches,
                    FailedFetches = statsDto.FailedFetches,
                    SuccessRate = (double)statsDto.SuccessRate,
                    FirstFetchDate = statsDto.FirstFetchDate.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(statsDto.FirstFetchDate.Value)
                        : null,
                    LastFetchDate = statsDto.LastFetchDate.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(statsDto.LastFetchDate.Value)
                        : null,
                    OldestRateDate = statsDto.OldestRateDate.HasValue
                        ? new gRPC.Protos.Common.Date
                        {
                            Year = statsDto.OldestRateDate.Value.Year,
                            Month = statsDto.OldestRateDate.Value.Month,
                            Day = statsDto.OldestRateDate.Value.Day
                        }
                        : null,
                    NewestRateDate = statsDto.NewestRateDate.HasValue
                        ? new gRPC.Protos.Common.Date
                        {
                            Year = statsDto.NewestRateDate.Value.Year,
                            Month = statsDto.NewestRateDate.Value.Month,
                            Day = statsDto.NewestRateDate.Value.Day
                        }
                        : null
                }
            };
        }

        return new GetProviderStatisticsResponse
        {
            Success = false,
            Message = $"Statistics for provider {request.Code} not found"
        };
    }

    public override async Task<GetProviderConfigurationResponse> GetProviderConfiguration(
        GetProviderConfigurationRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetProviderConfiguration request: {Code}", request.Code);

        // Get provider ID
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new GetProviderConfigurationResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var query = new GetProviderConfigurationQuery(provider.Id);
        var detailDto = await _mediator.Send(query, context.CancellationToken);

        if (detailDto != null)
        {
            return new GetProviderConfigurationResponse
            {
                Success = true,
                Message = $"Configuration for provider {request.Code} retrieved successfully",
                Data = new ProviderDetailInfo
                {
                    Id = detailDto.Id,
                    Code = detailDto.Code,
                    Name = detailDto.Name,
                    Url = detailDto.Url,
                    IsActive = detailDto.IsActive,
                    BaseCurrencyCode = detailDto.BaseCurrencyCode,
                    RequiresAuthentication = detailDto.RequiresAuthentication,
                    ApiKeyVaultReference = detailDto.ApiKeyVaultReference ?? "",
                    Status = detailDto.Status,
                    ConsecutiveFailures = detailDto.ConsecutiveFailures,
                    LastSuccessfulFetch = detailDto.LastSuccessfulFetch.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(detailDto.LastSuccessfulFetch.Value)
                        : null,
                    LastFailedFetch = detailDto.LastFailedFetch.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(detailDto.LastFailedFetch.Value)
                        : null,
                    Created = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(detailDto.Created),
                    Modified = detailDto.Modified.HasValue
                        ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(detailDto.Modified.Value)
                        : null
                }
            };
        }

        return new GetProviderConfigurationResponse
        {
            Success = false,
            Message = $"Configuration for provider {request.Code} not found"
        };
    }

    // ============================================================
    // COMMAND OPERATIONS (Admin Only)
    // ============================================================

    [Authorize(Roles = "Admin")]
    public override async Task<ActivateProviderResponse> ActivateProvider(
        ActivateProviderRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("ActivateProvider request: {Code}", request.Code);

        // Get provider ID
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new ActivateProviderResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var command = new ActivateProviderCommand(provider.Id);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new ActivateProviderResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? $"Provider {request.Code} activated successfully"
                : result.Error ?? "Failed to activate provider",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("ACTIVATION_ERROR", result.Error ?? "Failed to activate provider")
                : null
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<DeactivateProviderResponse> DeactivateProvider(
        DeactivateProviderRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("DeactivateProvider request: {Code}", request.Code);

        // Get provider ID
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new DeactivateProviderResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var command = new DeactivateProviderCommand(provider.Id);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new DeactivateProviderResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? $"Provider {request.Code} deactivated successfully"
                : result.Error ?? "Failed to deactivate provider",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("DEACTIVATION_ERROR", result.Error ?? "Failed to deactivate provider")
                : null
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<ResetProviderHealthResponse> ResetProviderHealth(
        ResetProviderHealthRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("ResetProviderHealth request: {Code}", request.Code);

        // Get provider ID
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new ResetProviderHealthResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var command = new ResetProviderHealthCommand(provider.Id);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new ResetProviderHealthResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? $"Health status for provider {request.Code} reset successfully"
                : result.Error ?? "Failed to reset provider health",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("RESET_ERROR", result.Error ?? "Failed to reset provider health")
                : null
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<TriggerManualFetchResponse> TriggerManualFetch(
        TriggerManualFetchRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("TriggerManualFetch request: {Code}", request.Code);

        // Get provider ID
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new TriggerManualFetchResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var command = new TriggerManualFetchCommand(provider.Id);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new TriggerManualFetchResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? $"Manual fetch triggered for provider {request.Code}"
                : result.Error ?? "Failed to trigger manual fetch",
            Data = result.IsSuccess ? result.Value ?? "" : "",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("FETCH_ERROR", result.Error ?? "Failed to trigger manual fetch")
                : null
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<CreateProviderResponse> CreateProvider(
        CreateProviderRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("CreateProvider request: {Code}", request.Code);

        var command = new CreateExchangeRateProviderCommand(
            request.Name,
            request.Code,
            request.Url,
            request.BaseCurrencyId,
            request.RequiresAuthentication,
            request.ApiKeyVaultReference);

        var result = await _mediator.Send(command, context.CancellationToken);

        if (result.IsSuccess && result.Value > 0)
        {
            // Query for the created provider
            var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
            var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

            var provider = pagedResult.Items.FirstOrDefault(p =>
                p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

            if (provider != null)
            {
                return new CreateProviderResponse
                {
                    Success = true,
                    Message = $"Provider {request.Code} created successfully",
                    Data = new ProviderInfo
                    {
                        Id = provider.Id,
                        Code = provider.Code,
                        Name = provider.Name,
                        Url = provider.Url,
                        IsActive = provider.IsActive,
                        BaseCurrencyCode = provider.BaseCurrencyCode,
                        Status = provider.Status,
                        ConsecutiveFailures = provider.ConsecutiveFailures,
                        LastSuccessfulFetch = provider.LastSuccessfulFetch.HasValue
                            ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.LastSuccessfulFetch.Value)
                            : null,
                        LastFailedFetch = provider.LastFailedFetch.HasValue
                            ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.LastFailedFetch.Value)
                            : null,
                        Created = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTimeOffset(provider.Created)
                    }
                };
            }
        }

        return new CreateProviderResponse
        {
            Success = false,
            Message = result.Error ?? "Failed to create provider",
            Error = CommonMappers.ToProtoError("CREATE_ERROR", result.Error ?? "Failed to create provider")
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<UpdateProviderConfigurationResponse> UpdateProviderConfiguration(
        UpdateProviderConfigurationRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("UpdateProviderConfiguration request: {Code}", request.Code);

        // Get provider ID
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new UpdateProviderConfigurationResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var command = new UpdateProviderConfigurationCommand(
            provider.Id,
            request.Name,
            request.Url,
            request.RequiresAuthentication,
            request.ApiKeyVaultReference);

        var result = await _mediator.Send(command, context.CancellationToken);

        return new UpdateProviderConfigurationResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? $"Configuration for provider {request.Code} updated successfully"
                : result.Error ?? "Failed to update provider configuration",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("UPDATE_ERROR", result.Error ?? "Failed to update provider configuration")
                : null
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<DeleteProviderResponse> DeleteProvider(
        DeleteProviderRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("DeleteProvider request: {Code}, Force: {Force}", request.Code, request.Force);

        // Get provider ID
        var providersQuery = new GetAllProvidersQuery(1, 100, null, request.Code);
        var pagedResult = await _mediator.Send(providersQuery, context.CancellationToken);

        var provider = pagedResult.Items.FirstOrDefault(p =>
            p.Code.Equals(request.Code, StringComparison.OrdinalIgnoreCase));

        if (provider == null)
        {
            return new DeleteProviderResponse
            {
                Success = false,
                Message = $"Provider with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Provider '{request.Code}' not found")
            };
        }

        var command = new DeleteProviderCommand(provider.Id, request.Force);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new DeleteProviderResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? $"Provider {request.Code} deleted successfully"
                : result.Error ?? "Failed to delete provider",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("DELETE_ERROR", result.Error ?? "Failed to delete provider")
                : null
        };
    }
}
