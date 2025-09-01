using ExchangeRateUpdater.Provider.Cnb.Dtos;
using System.Net;

namespace ExchangeRateUpdater.Provider.Cnb.Client
{
    public interface ICnbClient
    {
        /// <summary>
        /// Calls the Czech National Bank API and returns the exchange rates against the CZK.  
        /// </summary>
        /// <returns>The raw CNB response, or null if it cannot be deserialized.</returns>
        /// <exception cref="HttpRequestException">Thrown when the HTTP request fails 
        /// or the API returns a non-success status code.</exception>        
        Task<CnbResponse?> GetDailyRatesAsync();
    }
}
