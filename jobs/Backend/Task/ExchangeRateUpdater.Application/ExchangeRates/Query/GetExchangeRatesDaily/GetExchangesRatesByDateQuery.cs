namespace ExchangeRateUpdater.Application.ExchangeRates.Query.GetExchangeRatesDaily;

using Common.Models;
using Domain.Enums;
using Dtos;
using MediatR;

public class GetExchangesRatesByDateQuery : IRequest<Result<List<ExchangeRateDto>>>
{
    /// <summary>
    /// List of three-letter ISO 4217 currency codes for which exchange rates are requested.
    /// If empty, all available rates are fetched.
    /// </summary>
    public List<string>? CurrencyCodes { get; set; }

    /// <summary>
    /// Date for which exchange rates are requested.
    /// If null, the latest available rates are fetched.
    /// </summary>
    public DateTime? Date { get; set; }
    
    /// <summary>
    /// Language enumeration; default value: CZ
    /// </summary>
    public Language? Language { get; set; }
}