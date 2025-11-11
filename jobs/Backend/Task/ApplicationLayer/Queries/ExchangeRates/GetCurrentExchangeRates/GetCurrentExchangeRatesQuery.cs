using ApplicationLayer.Common.Abstractions;
using ApplicationLayer.DTOs.ExchangeRates;

namespace ApplicationLayer.Queries.ExchangeRates.GetCurrentExchangeRates;

/// <summary>
/// Query to get current exchange rates.
/// Note: For optimized read operations, this would ideally use Dapper/Views.
/// </summary>
public record GetCurrentExchangeRatesQuery : IQuery<IEnumerable<CurrentExchangeRateDto>>;
