using ApplicationLayer.Queries.ExchangeRates.GetAllLatestExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;
using ApplicationLayer.Queries.ExchangeRates.ConvertCurrency;
using ApplicationLayer.Queries.ExchangeRates.GetLatestExchangeRate;
using ApplicationLayer.Queries.ExchangeRates.GetExchangeRateHistory;
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

    public async Task<GetLatestRateResponse> GetLatestRateAsync(GetLatestRateRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetLatestRate called for {Source} to {Target}, ProviderId: {ProviderId}",
                request.SourceCurrency, request.TargetCurrency, request.ProviderId);

            var query = new GetLatestExchangeRateQuery(
                request.SourceCurrency,
                request.TargetCurrency,
                request.ProviderId
            );

            var rateDto = await _mediator.Send(query);

            if (rateDto != null)
            {
                return new GetLatestRateResponse
                {
                    Success = true,
                    Message = "Latest exchange rate retrieved successfully",
                    Data = rateDto.ToSoap()
                };
            }

            return new GetLatestRateResponse
            {
                Success = false,
                Message = $"No exchange rate found for {request.SourceCurrency} to {request.TargetCurrency}",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"No exchange rate found for {request.SourceCurrency} to {request.TargetCurrency}"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetLatestRate SOAP operation");

            return new GetLatestRateResponse
            {
                Success = false,
                Message = "An error occurred while retrieving the latest exchange rate",
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetAllLatestRatesResponse> GetAllLatestRatesAsync(GetAllLatestRatesRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetAllLatestRates called");

            var query = new GetAllLatestExchangeRatesQuery();
            var rates = await _mediator.Send(query);

            return new GetAllLatestRatesResponse
            {
                Success = true,
                Message = "All latest exchange rates retrieved successfully",
                Data = rates.ToFlatSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllLatestRates SOAP operation");

            return new GetAllLatestRatesResponse
            {
                Success = false,
                Message = "An error occurred while retrieving all latest exchange rates",
                Data = Array.Empty<Models.ExchangeRates.LatestExchangeRateSoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetCurrentRatesFlatResponse> GetCurrentRatesFlatAsync(GetCurrentRatesFlatRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetCurrentRatesFlat called");

            var query = new GetCurrentExchangeRatesQuery();
            var rates = await _mediator.Send(query);

            return new GetCurrentRatesFlatResponse
            {
                Success = true,
                Message = "Current exchange rates retrieved successfully",
                Data = rates.ToFlatSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCurrentRatesFlat SOAP operation");

            return new GetCurrentRatesFlatResponse
            {
                Success = false,
                Message = "An error occurred while retrieving current exchange rates",
                Data = Array.Empty<Models.ExchangeRates.CurrentExchangeRateSoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetHistoryResponse> GetHistoryAsync(GetHistoryRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetHistory called for {Source} to {Target}, from {Start} to {End}",
                request.SourceCurrency, request.TargetCurrency, request.StartDate, request.EndDate);

            // Parse dates
            if (!DateOnly.TryParse(request.StartDate, out var startDate))
            {
                return new GetHistoryResponse
                {
                    Success = false,
                    Message = "Invalid start date format. Use YYYY-MM-DD",
                    Data = Array.Empty<Models.ExchangeRates.ExchangeRateHistorySoap>(),
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "ValidationError",
                        Detail = "Invalid start date format. Use YYYY-MM-DD"
                    }
                };
            }

            if (!DateOnly.TryParse(request.EndDate, out var endDate))
            {
                return new GetHistoryResponse
                {
                    Success = false,
                    Message = "Invalid end date format. Use YYYY-MM-DD",
                    Data = Array.Empty<Models.ExchangeRates.ExchangeRateHistorySoap>(),
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "ValidationError",
                        Detail = "Invalid end date format. Use YYYY-MM-DD"
                    }
                };
            }

            var query = new GetExchangeRateHistoryQuery(
                request.SourceCurrency,
                request.TargetCurrency,
                startDate,
                endDate,
                request.ProviderId
            );

            var history = await _mediator.Send(query);

            return new GetHistoryResponse
            {
                Success = true,
                Message = "Exchange rate history retrieved successfully",
                Data = history.ToFlatSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetHistory SOAP operation");

            return new GetHistoryResponse
            {
                Success = false,
                Message = "An error occurred while retrieving exchange rate history",
                Data = Array.Empty<Models.ExchangeRates.ExchangeRateHistorySoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetHistoryGroupedResponse> GetHistoryGroupedAsync(GetHistoryGroupedRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetHistoryGrouped called for {Source} to {Target}, from {Start} to {End}",
                request.SourceCurrency, request.TargetCurrency, request.StartDate, request.EndDate);

            // Parse dates
            if (!DateOnly.TryParse(request.StartDate, out var startDate))
            {
                return new GetHistoryGroupedResponse
                {
                    Success = false,
                    Message = "Invalid start date format. Use YYYY-MM-DD",
                    Data = Array.Empty<Models.ExchangeRates.ExchangeRateHistoryGroupedSoap>(),
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "ValidationError",
                        Detail = "Invalid start date format. Use YYYY-MM-DD"
                    }
                };
            }

            if (!DateOnly.TryParse(request.EndDate, out var endDate))
            {
                return new GetHistoryGroupedResponse
                {
                    Success = false,
                    Message = "Invalid end date format. Use YYYY-MM-DD",
                    Data = Array.Empty<Models.ExchangeRates.ExchangeRateHistoryGroupedSoap>(),
                    Fault = new SoapFault
                    {
                        FaultCode = "Client",
                        FaultString = "ValidationError",
                        Detail = "Invalid end date format. Use YYYY-MM-DD"
                    }
                };
            }

            var query = new GetExchangeRateHistoryQuery(
                request.SourceCurrency,
                request.TargetCurrency,
                startDate,
                endDate,
                request.ProviderId
            );

            var history = await _mediator.Send(query);

            return new GetHistoryGroupedResponse
            {
                Success = true,
                Message = "Exchange rate history (grouped) retrieved successfully",
                Data = history.ToGroupedSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetHistoryGrouped SOAP operation");

            return new GetHistoryGroupedResponse
            {
                Success = false,
                Message = "An error occurred while retrieving exchange rate history",
                Data = Array.Empty<Models.ExchangeRates.ExchangeRateHistoryGroupedSoap>(),
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
