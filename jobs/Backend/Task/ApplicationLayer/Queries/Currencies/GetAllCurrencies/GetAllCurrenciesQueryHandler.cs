using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.Currencies;
using DomainLayer.Interfaces.Persistence;
using Microsoft.Extensions.Logging;

namespace ApplicationLayer.Queries.Currencies.GetAllCurrencies;

public class GetAllCurrenciesQueryHandler
    : IQueryHandler<GetAllCurrenciesQuery, PagedResult<CurrencyDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<GetAllCurrenciesQueryHandler> _logger;

    public GetAllCurrenciesQueryHandler(
        IUnitOfWork unitOfWork,
        ILogger<GetAllCurrenciesQueryHandler> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<PagedResult<CurrencyDto>> Handle(
        GetAllCurrenciesQuery request,
        CancellationToken cancellationToken)
    {
        _logger.LogDebug(
            "Getting all currencies with pagination (Page: {PageNumber}, Size: {PageSize}, Search: {SearchTerm})",
            request.PageNumber,
            request.PageSize,
            request.SearchTerm);

        // Get all currencies
        var currencies = await _unitOfWork.Currencies.GetAllAsync(cancellationToken);

        // Apply search filter if provided
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToUpperInvariant();
            currencies = currencies.Where(c => c.Code.Contains(searchTerm));
        }

        var currencyList = currencies.ToList();
        var totalCount = currencyList.Count;

        // Apply pagination if requested
        IReadOnlyCollection<CurrencyDto> items;
        if (request.IncludePagination)
        {
            items = currencyList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(MapToCurrencyDto)
                .ToList();
        }
        else
        {
            items = currencyList
                .Select(MapToCurrencyDto)
                .ToList();
        }

        _logger.LogDebug("Retrieved {Count} currencies (total: {TotalCount})", items.Count, totalCount);

        return PagedResult<CurrencyDto>.Create(
            items,
            totalCount,
            request.PageNumber,
            request.PageSize);
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
