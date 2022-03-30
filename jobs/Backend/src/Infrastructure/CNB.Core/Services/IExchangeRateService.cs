namespace ExchangeRate.Infrastructure.CNB.Core.Services;

/// <summary>
/// Service to work with HttpClient from CNB baseUrl source
/// </summary>
public interface IExchangeRateService
{
    /// <summary>
    /// Fetches CNB exchange rate data via http client
    /// </summary>
    /// <returns>HttpResponseMessage</returns>
    Task<HttpResponseMessage> FetchDataAsync();
}
