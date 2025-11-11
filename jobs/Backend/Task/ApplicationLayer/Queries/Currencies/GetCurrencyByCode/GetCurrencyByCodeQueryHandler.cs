using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Currencies;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Currencies.GetCurrencyByCode;

public class GetCurrencyByCodeQueryHandler
    : IQueryHandler<GetCurrencyByCodeQuery, CurrencyDto?>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetCurrencyByCodeQueryHandler> _logger;

    public GetCurrencyByCodeQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetCurrencyByCodeQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CurrencyDto?> Handle(
        GetCurrencyByCodeQuery request,
        CancellationToken cancellationToken)
    {
        var code = request.Code.ToUpperInvariant();

        _logger.LogDebug("Getting currency by code: {Code}", code);

        var currency = await _unitOfWork.Currencies.GetByCodeAsync(code, cancellationToken);

        if (currency == null)
        {
            _logger.LogDebug("Currency {Code} not found", code);
            return null;
        }

        _logger.LogDebug("Found currency {Code} with ID {Id}", currency.Code, currency.Id);

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
