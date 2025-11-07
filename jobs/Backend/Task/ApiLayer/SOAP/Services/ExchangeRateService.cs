using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
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
}
