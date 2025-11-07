using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetLatestExchangeRate;
using ApplicationLayer.Queries.ExchangeRates.GetExchangeRateHistory;
using ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;
using gRPC.Mappers;
using gRPC.Protos.ExchangeRates;
using gRPC.Streaming;
using Grpc.Core;
using MediatR;
using Microsoft.AspNetCore.Authorization;

namespace gRPC.Services;

/// <summary>
/// gRPC service for exchange rate operations and real-time streaming updates.
/// </summary>
[Authorize(Roles = "Consumer,Admin")]
public class ExchangeRatesGrpcService : ExchangeRatesService.ExchangeRatesServiceBase
{
    private readonly IMediator _mediator;
    private readonly IExchangeRatesStreamManager _streamManager;
    private readonly ILogger<ExchangeRatesGrpcService> _logger;

    public ExchangeRatesGrpcService(
        IMediator mediator,
        IExchangeRatesStreamManager streamManager,
        ILogger<ExchangeRatesGrpcService> logger)
    {
        _mediator = mediator;
        _streamManager = streamManager;
        _logger = logger;
    }

    // ============================================================
    // CURRENT RATES
    // ============================================================

    public override async Task<GetCurrentRatesResponse> GetCurrentRates(
        GetCurrentRatesRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetCurrentRates request received");

        var query = new GetCurrentExchangeRatesQuery();
        var rates = await _mediator.Send(query, context.CancellationToken);

        var response = new GetCurrentRatesResponse
        {
            Message = "Current exchange rates retrieved successfully"
        };
        response.Rates.AddRange(rates.Select(ExchangeRateMappers.ToProtoCurrentRate));

        return response;
    }

    public override async Task<GetCurrentRatesGroupedResponse> GetCurrentRatesGrouped(
        GetCurrentRatesRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetCurrentRatesGrouped request received");

        var query = new GetCurrentExchangeRatesQuery();
        var rates = await _mediator.Send(query, context.CancellationToken);

        // TODO: Implement grouped conversion for CurrentExchangeRateDto
        // For now, return empty grouped response
        var response = new GetCurrentRatesGroupedResponse
        {
            Message = "Current exchange rates (grouped) retrieved successfully"
        };

        return response;
    }

    // ============================================================
    // LATEST RATES
    // ============================================================

    public override async Task<GetLatestRateResponse> GetLatestRate(
        GetLatestRateRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "GetLatestRate request: {Source} -> {Target}, Provider: {ProviderId}",
            request.SourceCurrency,
            request.TargetCurrency,
            request.ProviderId);

        var query = new GetLatestExchangeRateQuery(
            request.SourceCurrency,
            request.TargetCurrency,
            request.HasProviderId ? request.ProviderId : null);

        var rateDto = await _mediator.Send(query, context.CancellationToken);

        if (rateDto != null)
        {
            return new GetLatestRateResponse
            {
                Success = true,
                Message = "Latest exchange rate retrieved successfully",
                Data = new ExchangeRateData
                {
                    ProviderId = rateDto.ProviderId,
                    ProviderCode = rateDto.ProviderCode,
                    ProviderName = rateDto.ProviderName,
                    SourceCurrencyCode = rateDto.BaseCurrencyCode,
                    TargetCurrencyCode = rateDto.TargetCurrencyCode,
                    Rate = rateDto.Rate.ToString("G29"), // Raw rate from provider
                    Multiplier = rateDto.Multiplier,
                    EffectiveRate = rateDto.EffectiveRate.ToString("G29"),
                    ValidDate = ExchangeRateMappers.ToProtoDate(rateDto.ValidDate)
                }
            };
        }

        return new GetLatestRateResponse
        {
            Success = false,
            Message = $"No exchange rate found for {request.SourceCurrency} to {request.TargetCurrency}"
        };
    }

    public override async Task<GetAllLatestRatesResponse> GetAllLatestRates(
        GetAllLatestRatesRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetAllLatestRates request received");

        var query = new GetAllLatestExchangeRatesQuery();
        var rates = await _mediator.Send(query, context.CancellationToken);

        var response = new GetAllLatestRatesResponse
        {
            Message = "All latest exchange rates retrieved successfully"
        };
        response.Rates.AddRange(rates.Select(ExchangeRateMappers.ToProtoLatestRate));

        return response;
    }

    public override async Task<GetAllLatestRatesGroupedResponse> GetAllLatestRatesGrouped(
        GetAllLatestRatesRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation("GetAllLatestRatesGrouped request received");

        var query = new GetAllLatestExchangeRatesQuery();
        var rates = await _mediator.Send(query, context.CancellationToken);

        // Convert to grouped proto structure
        var groupedData = ExchangeRateMappers.ToProtoGroupedData(rates);

        var response = new GetAllLatestRatesGroupedResponse
        {
            Message = "All latest exchange rates (grouped) retrieved successfully"
        };
        response.Providers.AddRange(groupedData.Providers);

        return response;
    }

    // ============================================================
    // HISTORY
    // ============================================================

    public override async Task<GetHistoryResponse> GetHistory(
        GetHistoryRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "GetHistory request: {Source} -> {Target}, {StartDate} to {EndDate}",
            request.SourceCurrency,
            request.TargetCurrency,
            request.StartDate,
            request.EndDate);

        var query = new GetExchangeRateHistoryQuery(
            request.SourceCurrency,
            request.TargetCurrency,
            ExchangeRateMappers.FromProtoDate(request.StartDate),
            ExchangeRateMappers.FromProtoDate(request.EndDate),
            request.HasProviderId ? request.ProviderId : null);

        var history = await _mediator.Send(query, context.CancellationToken);

        var response = new GetHistoryResponse
        {
            Message = "Exchange rate history retrieved successfully"
        };
        response.History.AddRange(history.Select(ExchangeRateMappers.ToProtoHistory));

        return response;
    }

    public override async Task<GetHistoryGroupedResponse> GetHistoryGrouped(
        GetHistoryRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "GetHistoryGrouped request: {Source} -> {Target}, {StartDate} to {EndDate}",
            request.SourceCurrency,
            request.TargetCurrency,
            request.StartDate,
            request.EndDate);

        var query = new GetExchangeRateHistoryQuery(
            request.SourceCurrency,
            request.TargetCurrency,
            ExchangeRateMappers.FromProtoDate(request.StartDate),
            ExchangeRateMappers.FromProtoDate(request.EndDate),
            request.HasProviderId ? request.ProviderId : null);

        var history = await _mediator.Send(query, context.CancellationToken);

        // TODO: Implement grouped history conversion
        var response = new GetHistoryGroupedResponse
        {
            Message = "Exchange rate history (grouped) retrieved successfully"
        };

        return response;
    }

    // ============================================================
    // CURRENCY CONVERSION
    // ============================================================

    public override async Task<ConvertCurrencyResponse> ConvertCurrency(
        ConvertCurrencyRequest request,
        ServerCallContext context)
    {
        _logger.LogInformation(
            "ConvertCurrency request: {Amount} {From} -> {To}",
            request.Amount,
            request.FromCurrency,
            request.ToCurrency);

        var amount = decimal.Parse(request.Amount);

        var query = new ConvertCurrencyQuery(
            request.FromCurrency,
            request.ToCurrency,
            amount,
            null, // ProviderId
            null  // Date
        );

        var conversionResult = await _mediator.Send(query, context.CancellationToken);

        if (conversionResult != null)
        {
            return new ConvertCurrencyResponse
            {
                Success = true,
                Message = "Currency conversion completed successfully",
                Data = ExchangeRateMappers.ToProtoConversionResult(
                    conversionResult.SourceCurrencyCode,
                    conversionResult.TargetCurrencyCode,
                    conversionResult.SourceAmount,
                    conversionResult.TargetAmount,
                    conversionResult.EffectiveRate,
                    conversionResult.ValidDate.ToString("yyyy-MM-dd"))
            };
        }

        return new ConvertCurrencyResponse
        {
            Success = false,
            Message = "Exchange rate not found for the specified currency pair"
        };
    }

    // ============================================================
    // STREAMING: PUSH NOTIFICATIONS (THE KEY FEATURE!)
    // ============================================================

    /// <summary>
    /// Server-side streaming RPC that replaces SignalR for push notifications.
    /// Clients subscribe and receive real-time updates when exchange rates change.
    /// </summary>
    public override async Task StreamExchangeRateUpdates(
        StreamSubscriptionRequest request,
        IServerStreamWriter<ExchangeRateUpdateEvent> responseStream,
        ServerCallContext context)
    {
        var clientId = Guid.NewGuid().ToString();
        var userName = context.GetHttpContext().User?.Identity?.Name ?? "Anonymous";

        _logger.LogInformation(
            "Client {ClientId} ({User}) subscribed to exchange rate updates. Subscriptions: {Subscriptions}",
            clientId,
            userName,
            string.Join(", ", request.SubscriptionTypes));

        try
        {
            // Register client with the stream manager
            await _streamManager.RegisterClientAsync(
                clientId,
                responseStream,
                request.SubscriptionTypes,
                context.CancellationToken);

            // Send current rates to new subscriber immediately
            try
            {
                _logger.LogInformation("Sending current rates to new subscriber: {ClientId}", clientId);

                var query = new GetAllLatestExchangeRatesQuery();
                var rates = await _mediator.Send(query, context.CancellationToken);

                if (rates.Any())
                {
                    var protoData = ExchangeRateMappers.ToProtoGroupedData(rates);
                    var updateEvent = new ExchangeRateUpdateEvent
                    {
                        EventType = "LatestRatesUpdated",
                        Timestamp = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.UtcNow),
                        Data = protoData
                    };

                    await responseStream.WriteAsync(updateEvent, context.CancellationToken);
                    _logger.LogInformation("Current rates sent to new subscriber: {ClientId}", clientId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending current rates to new subscriber: {ClientId}", clientId);
            }

            // Keep the connection alive indefinitely
            // The stream manager will push updates to this client when events occur
            await Task.Delay(Timeout.Infinite, context.CancellationToken);
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation(
                "Client {ClientId} ({User}) cancelled subscription",
                clientId,
                userName);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error in streaming connection for client {ClientId}",
                clientId);
            throw;
        }
        finally
        {
            // Cleanup: unregister client when connection closes
            await _streamManager.UnregisterClientAsync(clientId);
            _logger.LogInformation(
                "Client {ClientId} ({User}) disconnected from streaming",
                clientId,
                userName);
        }
    }
}
