using ApplicationLayer.Commands.Currencies.CreateCurrency;
using ApplicationLayer.Commands.Currencies.DeleteCurrency;
using ApplicationLayer.Queries.Currencies.GetAllCurrencies;
using ApplicationLayer.Queries.Currencies.GetCurrencyByCode;
using ApplicationLayer.Queries.Currencies.GetCurrencyById;
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
            var query = new GetAllCurrenciesQuery(PageNumber: 1, PageSize: 100, IncludePagination: false);
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

    public async Task<GetCurrencyByIdResponse> GetCurrencyByIdAsync(GetCurrencyByIdRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: GetCurrencyById called for ID: {Id}", request.Id);

            // Reuse existing ApplicationLayer query
            var query = new GetCurrencyByIdQuery(request.Id);
            var currency = await _mediator.Send(query);

            if (currency != null)
            {
                return new GetCurrencyByIdResponse
                {
                    Success = true,
                    Message = $"Currency with ID {request.Id} retrieved successfully",
                    Data = currency.ToSoap()
                };
            }

            return new GetCurrencyByIdResponse
            {
                Success = false,
                Message = $"Currency with ID {request.Id} not found",
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "NotFound",
                    Detail = $"Currency with ID {request.Id} not found"
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCurrencyById SOAP operation");

            return new GetCurrencyByIdResponse
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

    [Authorize(Roles = "Admin")]
    public async Task<CreateCurrencyResponse> CreateCurrencyAsync(CreateCurrencyRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: CreateCurrency called for code: {Code}", request.Code);

            var command = new CreateCurrencyCommand(request.Code);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                // Fetch the created currency to return it
                var query = new GetCurrencyByCodeQuery(request.Code);
                var currency = await _mediator.Send(query);

                return new CreateCurrencyResponse
                {
                    Success = true,
                    Message = $"Currency {request.Code} created successfully",
                    Data = currency?.ToSoap()
                };
            }

            return new CreateCurrencyResponse
            {
                Success = false,
                Message = result.Error,
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ValidationError",
                    Detail = result.Error
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in CreateCurrency SOAP operation");

            return new CreateCurrencyResponse
            {
                Success = false,
                Message = "An error occurred while creating currency",
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
    public async Task<DeleteCurrencyResponse> DeleteCurrencyAsync(DeleteCurrencyRequest request)
    {
        try
        {
            _logger.LogInformation("SOAP: DeleteCurrency called for code: {Code}", request.Code);

            // First get currency by code to get its ID
            var getCurrencyQuery = new GetCurrencyByCodeQuery(request.Code);
            var currency = await _mediator.Send(getCurrencyQuery);

            if (currency == null)
            {
                return new DeleteCurrencyResponse
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

            var command = new DeleteCurrencyCommand(currency.Id);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                return new DeleteCurrencyResponse
                {
                    Success = true,
                    Message = $"Currency {request.Code} deleted successfully"
                };
            }

            return new DeleteCurrencyResponse
            {
                Success = false,
                Message = result.Error,
                Fault = new SoapFault
                {
                    FaultCode = "Client",
                    FaultString = "ValidationError",
                    Detail = result.Error
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in DeleteCurrency SOAP operation");

            return new DeleteCurrencyResponse
            {
                Success = false,
                Message = "An error occurred while deleting currency",
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
