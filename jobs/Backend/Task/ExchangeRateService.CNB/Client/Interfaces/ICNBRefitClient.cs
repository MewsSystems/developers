using ExchangeRateService.CNB.Client.Model;
using Refit;

namespace ExchangeRateService.CNB.Client.Interfaces;

/// <summary>
/// Represents a rest api interface of the CNB API
/// </summary>
public interface ICNBRefitClient
{
    /// <summary>
    /// Gets the exchange rate for daily refreshed currencies supported by CNB
    /// </summary>
    /// <param name="date">Date for which we want to know the rate</param>
    /// <param name="lang">Language of the output data</param>
    /// <returns>Class representing the HTTP response</returns>
    [Get("/cnbapi/exrates/daily")]
    Task<ExratesDailyResponse> GetExratesDailyRates([Query(Format = "yyyy-MM-dd")] DateTime date, [Query] CNBLanguage lang = CNBLanguage.EN);
    
    /// <summary>
    /// Gets the exchange rate for monthly refreshed currencies supported by CNB
    /// </summary>
    /// <param name="yearMonth">Month for which we want to know the rate</param>
    /// <param name="lang">Language of the output data</param>
    /// <returns>Class representing the HTTP response</returns>
    [Get("/cnbapi/fxrates/daily-month")]
    Task<FXRatesDailyMonthResponse> GetFXRatesDailyMonthRates([Query(Format = "yyyy-MM")] DateTime yearMonth, [Query] CNBLanguage lang = CNBLanguage.EN);
}