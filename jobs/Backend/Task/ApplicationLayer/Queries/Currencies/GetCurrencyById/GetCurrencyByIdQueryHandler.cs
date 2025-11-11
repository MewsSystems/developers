using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Currencies;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Currencies.GetCurrencyById;

public class GetCurrencyByIdQueryHandler
    : IQueryHandler<GetCurrencyByIdQuery, CurrencyDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCurrencyByIdQueryHandler> _logger;

    public GetCurrencyByIdQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCurrencyByIdQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CurrencyDto?> Handle(
        GetCurrencyByIdQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug("Getting currency by ID: {CurrencyId}", request.CurrencyId);

        var currency = await _unitOfWork.Currencies.GetByIdAsync(request.CurrencyId, cancellationToken);

        if (currency == null)
        {
            _logger.LogDebug("Currency {CurrencyId} not found", request.CurrencyId);
            return null;
        }

        _logger.LogDebug("Found currency {Code}", currency.Code);

        return MapToCurrencyDto(currency);
    }

    private static CurrencyDto MapToCurrencyDto(DomainLayer.ValueObjects.Currency currency)
    {
        return new CurrencyDto
        {
            Id = currency.Id,
            Code = currency.Code
        };
    }
}
