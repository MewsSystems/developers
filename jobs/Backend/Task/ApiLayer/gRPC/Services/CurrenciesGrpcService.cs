using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.Currencies.DeleteCurrency;
using ApplicationLayer.Queries.Currencies.GetAllCurrencies;
using ApplicationLayer.Queries.Currencies.GetCurrencyByCode;
using ApplicationLayer.Queries.Currencies.GetCurrencyById;
using gRPC.Mappers;
using gRPC.Protos.Currencies;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace gRPC.Services;

/// <summary>
/// gRPC service for currency management operations.
/// </summary>
[Authorize(Roles = "Consumer,Admin")]
public class CurrenciesGrpcService : CurrenciesService.CurrenciesServiceBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CurrenciesGrpcService> _logger;

    public CurrenciesGrpcService(
        IMediator mediator,
        ILogger<CurrenciesGrpcService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public override async Task<GetAllCurrenciesResponse> GetAllCurrencies(
        GetAllCurrenciesRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetAllCurrencies request received");

        var query = new GetAllCurrenciesQuery(IncludePagination: false);
        var currencies = await _mediator.Send(query, context.CancellationToken);

        var response = new GetAllCurrenciesResponse
        {
            Message = "Currencies retrieved successfully"
        };

        foreach (var currency in currencies.Items)
        {
            response.Currencies.Add(new CurrencyInfo
            {
                Id = currency.Id,
                Code = currency.Code
            });
        }

        return response;
    }

    public override async Task<GetCurrencyByIdResponse> GetCurrencyById(
        GetCurrencyByIdRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetCurrencyById request: {Id}", request.Id);

        var query = new GetCurrencyByIdQuery(request.Id);
        var currency = await _mediator.Send(query, context.CancellationToken);

        if (currency != null)
        {
            return new GetCurrencyByIdResponse
            {
                Success = true,
                Message = "Currency retrieved successfully",
                Data = new CurrencyInfo
                {
                    Id = currency.Id,
                    Code = currency.Code
                }
            };
        }

        return new GetCurrencyByIdResponse
        {
            Success = false,
            Message = $"Currency with ID {request.Id} not found",
            Error = CommonMappers.ToProtoError("NOT_FOUND", $"Currency with ID {request.Id} not found")
        };
    }

    public override async Task<GetCurrencyByCodeResponse> GetCurrencyByCode(
        GetCurrencyByCodeRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetCurrencyByCode request: {Code}", request.Code);

        var query = new GetCurrencyByCodeQuery(request.Code);
        var currency = await _mediator.Send(query, context.CancellationToken);

        if (currency != null)
        {
            return new GetCurrencyByCodeResponse
            {
                Success = true,
                Message = "Currency retrieved successfully",
                Data = new CurrencyInfo
                {
                    Id = currency.Id,
                    Code = currency.Code
                }
            };
        }

        return new GetCurrencyByCodeResponse
        {
            Success = false,
            Message = $"Currency with code '{request.Code}' not found",
            Error = CommonMappers.ToProtoError("NOT_FOUND", $"Currency with code '{request.Code}' not found")
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<CreateCurrencyResponse> CreateCurrency(
        CreateCurrencyRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("CreateCurrency request: {Code}", request.Code);

        var command = new CreateCurrencyCommand(request.Code);
        var result = await _mediator.Send(command, context.CancellationToken);

        if (result.IsSuccess && result.Value > 0)
        {
            // Query for the created currency
            var query = new GetCurrencyByIdQuery(result.Value);
            var currency = await _mediator.Send(query, context.CancellationToken);

            if (currency != null)
            {
                return new CreateCurrencyResponse
                {
                    Success = true,
                    Message = $"Currency {request.Code} created successfully",
                    Data = new CurrencyInfo
                    {
                        Id = currency.Id,
                        Code = currency.Code
                    }
                };
            }
        }

        return new CreateCurrencyResponse
        {
            Success = false,
            Message = result.Error ?? "Failed to create currency",
            Error = CommonMappers.ToProtoError("CREATE_ERROR", result.Error ?? "Failed to create currency")
        };
    }

    [Authorize(Roles = "Admin")]
    public override async Task<DeleteCurrencyResponse> DeleteCurrency(
        DeleteCurrencyRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("DeleteCurrency request: {Code}", request.Code);

        // First, lookup the currency by code to get its ID
        var query = new GetCurrencyByCodeQuery(request.Code);
        var currency = await _mediator.Send(query, context.CancellationToken);

        if (currency == null)
        {
            return new DeleteCurrencyResponse
            {
                Success = false,
                Message = $"Currency with code '{request.Code}' not found",
                Error = CommonMappers.ToProtoError("NOT_FOUND", $"Currency with code '{request.Code}' not found")
            };
        }

        var command = new DeleteCurrencyCommand(currency.Id);
        var result = await _mediator.Send(command, context.CancellationToken);

        return new DeleteCurrencyResponse
        {
            Success = result.IsSuccess,
            Message = result.IsSuccess
                ? $"Currency {request.Code} deleted successfully"
                : result.Error ?? "Failed to delete currency",
            Error = !result.IsSuccess
                ? CommonMappers.ToProtoError("DELETE_ERROR", result.Error ?? "Failed to delete currency")
                : null
        };
    }
}
