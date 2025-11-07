using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SOAP.Converters;

namespace SOAP.Services;

/// <summary>
/// SOAP service implementation for exchange rate operations.
/// Reuses ApplicationLayer queries via MediatR.
/// </summary>
[Authorize(Roles = "Consumer,Admin")]
public class ExchangeRateService : IExchangeRateService
{
    private readonly IMediator _mediator;
    private readonly ILogger<ExchangeRateService> _logger;

    public ExchangeRateService(
        IMediator mediator,
        ILogger<ExchangeRateService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<GetAllLatestRatesGroupedResponse> GetAllLatestRatesGroupedAsync(
        GetAllLatestRatesGroupedRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetAllLatestRatesGrouped called");

            // Reuse existing ApplicationLayer query
            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query);

            // Convert to SOAP grouped response
            var groupedRates = rates.ToNestedGroupedSoap();

            return new GetAllLatestRatesGroupedResponse
            {
                Success = true,
                Message = "All latest exchange rates retrieved successfully",
                Data = groupedRates
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllLatestRatesGrouped SOAP operation");

            return new GetAllLatestRatesGroupedResponse
            {
                Success = false,
                Message = "An error occurred while retrieving exchange rates",
                Data = Array.Empty<Models.ExchangeRates.LatestExchangeRatesGroupedSoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetAllLatestRatesGroupedResponse> GetHistoricalRatesUpdateAsync(
        GetAllLatestRatesGroupedRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetHistoricalRatesUpdate called");

            // Same as GetAllLatestRatesGrouped - provides latest rates for historical context
            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query);

            // Convert to SOAP grouped response
            var groupedRates = rates.ToNestedGroupedSoap();

            return new GetAllLatestRatesGroupedResponse
            {
                Success = true,
                Message = "Historical rates update retrieved successfully",
                Data = groupedRates
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetHistoricalRatesUpdate SOAP operation");

            return new GetAllLatestRatesGroupedResponse
            {
                Success = false,
                Message = "An error occurred while retrieving historical rates update",
                Data = Array.Empty<Models.ExchangeRates.LatestExchangeRatesGroupedSoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetCurrentRatesResponse> GetCurrentRatesAsync(GetCurrentRatesRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetCurrentRates called");

            // Reuse existing ApplicationLayer query
            var query = new GetCurrentExchangeRatesQuery();
            var rates = await _mediator.Send(query);

            // Convert to SOAP grouped response
            var groupedRates = rates.ToCurrentNestedGroupedSoap();

            return new GetCurrentRatesResponse
            {
                Success = true,
                Message = "Current exchange rates retrieved successfully",
                Data = groupedRates
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCurrentRates SOAP operation");

            return new GetCurrentRatesResponse
            {
                Success = false,
                Message = "An error occurred while retrieving current exchange rates",
                Data = Array.Empty<Models.ExchangeRates.CurrentExchangeRatesGroupedSoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<ConvertCurrencyResponse> ConvertCurrencyAsync(ConvertCurrencyRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: ConvertCurrency called for {From} to {To}, Amount: {Amount}",
                request.FromCurrency, request.ToCurrency, request.Amount);

            // Reuse existing ApplicationLayer query
            var query = new ConvertCurrencyQuery(
                request.FromCurrency,
                request.ToCurrency,
                request.Amount,
                null, // ProviderId - will use latest available
                null  // Date - will use latest available
            );

            var conversionResult = await _mediator.Send(query);

            if (conversionResult != null)
            {
                return new ConvertCurrencyResponse
                {
                    Success = true,
                    Message = "Currency conversion completed successfully",
                    Data = conversionResult.ToSoap()
                };
            }

            return new ConvertCurrencyResponse
            {
                Success = false,
                Message = "Exchange rate not found for the specified currency pair",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"No exchange rate found for {request.FromCurrency} to {request.ToCurrency}"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in ConvertCurrency SOAP operation");

            return new ConvertCurrencyResponse
            {
                Success = false,
                Message = "An error occurred while converting currency",
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
