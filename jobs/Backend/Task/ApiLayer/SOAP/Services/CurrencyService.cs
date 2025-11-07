using ApplicationLayer.Queries.Currencies.GetAllCurrencies;
using ApplicationLayer.Queries.Currencies.GetCurrencyByCode;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using SOAP.Converters;

namespace SOAP.Services;

/// <summary>
/// SOAP service implementation for currency operations.
/// Reuses ApplicationLayer queries via MediatR.
/// </summary>
[Authorize(Roles = "Consumer,Admin")]
public class CurrencyService : ICurrencyService
{
    private readonly IMediator _mediator;
    private readonly ILogger<CurrencyService> _logger;

    public CurrencyService(
        IMediator mediator,
        ILogger<CurrencyService> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task<GetAllCurrenciesResponse> GetAllCurrenciesAsync(GetAllCurrenciesRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetAllCurrencies called");

            // Reuse existing ApplicationLayer query
            var query = new GetAllCurrenciesQuery(PageNumber: 1, PageSize: 1000, IncludePagination: false);
            var pagedResult = await _mediator.Send(query);

            return new GetAllCurrenciesResponse
            {
                Success = true,
                Message = "Currencies retrieved successfully",
                Data = pagedResult.Items.ToSoap()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetAllCurrencies SOAP operation");

            return new GetAllCurrenciesResponse
            {
                Success = false,
                Message = "An error occurred while retrieving currencies",
                Data = Array.Empty<Models.Currencies.CurrencySoap>(),
                Fault = new SoapFault
                {
                    FaultCode = "Server",
                    FaultString = "Internal server error",
                    Detail = ex.Message
                }
            };
        }
    }

    public async Task<GetCurrencyByCodeResponse> GetCurrencyByCodeAsync(GetCurrencyByCodeRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetCurrencyByCode called for code: {Code}", request.Code);

            // Reuse existing ApplicationLayer query
            var query = new GetCurrencyByCodeQuery(request.Code);
            var currency = await _mediator.Send(query);

            if (currency != null)
            {
                return new GetCurrencyByCodeResponse
                {
                    Success = true,
                    Message = $"Currency {request.Code} retrieved successfully",
                    Data = currency.ToSoap()
                };
            }

            return new GetCurrencyByCodeResponse
            {
                Success = false,
                Message = $"Currency with code '{request.Code}' not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Currency with code '{request.Code}' not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCurrencyByCode SOAP operation");

            return new GetCurrencyByCodeResponse
            {
                Success = false,
                Message = "An error occurred while retrieving currency",
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
