using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.Common;
using ApplicationLayer.DTOs.ExchangeRateProviders;
using DomainLayer.Interfaces.Persistence;

namespace ApplicationLayer.Queries.Providers.GetAllProviders;

/// <summary>
/// Handler for retrieving all providers with pagination and filtering.
/// </summary>
public class GetAllProvidersQueryHandler : IQueryHandler<GetAllProvidersQuery, PagedResult<ExchangeRateProviderDto>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAllProvidersQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<PagedResult<ExchangeRateProviderDto>> Handle(
        GetAllProvidersQuery request,
        CancellationToken cancellationToken)
    {
        // Get all providers
        var allProviders = await _unitOfWork.ExchangeRateProviders
            .GetAllAsync(cancellationToken);

        // Apply filtering
        var filteredProviders = allProviders.AsEnumerable();

        if (request.IsActive.HasValue)
        {
            filteredProviders = filteredProviders.Where(p => p.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var searchTerm = request.SearchTerm.ToLowerInvariant();
            filteredProviders = filteredProviders.Where(p =>
                p.Name.ToLowerInvariant().Contains(searchTerm) ||
                p.Code.ToLowerInvariant().Contains(searchTerm) ||
                p.Url.ToLowerInvariant().Contains(searchTerm));
        }

        var totalCount = filteredProviders.Count();

        // Apply pagination
        var pagedProviders = filteredProviders
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        // Get all currencies for mapping
        var currencies = await _unitOfWork.Currencies
            .GetAllAsync(cancellationToken);

        var currencyDict = currencies.ToDictionary(c => c.Id, c => c.Code);

        // Map to DTOs
        var dtos = pagedProviders.Select(p => new ExchangeRateProviderDto
        {
            Id = p.Id,
            Name = p.Name,
            Code = p.Code,
            Url = p.Url,
            BaseCurrencyId = p.BaseCurrencyId,
            BaseCurrencyCode = currencyDict.TryGetValue(p.BaseCurrencyId, out var code) ? code : "UNKNOWN",
            IsActive = p.IsActive,
            Status = p.Status.ToString(),
            ConsecutiveFailures = p.ConsecutiveFailures,
            LastSuccessfulFetch = p.LastSuccessfulFetch,
            LastFailedFetch = p.LastFailedFetch,
            Created = p.Created
        }).ToList();

        return PagedResult<ExchangeRateProviderDto>.Create(
            dtos,
            totalCount,
            request.PageNumber,
            request.PageSize);
    }
}
