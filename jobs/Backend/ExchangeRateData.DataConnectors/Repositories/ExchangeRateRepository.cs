using ExchangeRateData.DataConnectors.Constants;
using ExchangeRateData.DataConnectors.Models;
using Microsoft.Extensions.Logging;

namespace ExchangeRateData.DataConnectors.Repositories;

public class ExchangeRateRepository : IExchangeRateRepository
{
    private readonly ILogger _logger;
    private readonly string _logName = "ExchangeRateRepository";

    public ExchangeRateRepository(ILogger<ExchangeRateRepository> logger)
    {
        _logger = logger;
    }

    
    /// <inheritdoc/>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesByCurrencyAndDateTaskAsync(string submittedDate, string [] currenciesArray)
    {
        DateTime startTime = DateTime.Now;
        string logHeader = _logName + ".GetExchangeRatesByCurrencyAndDateTaskAsync:";
        string responseBody = string.Empty;
        List<ExchangeRate>? listOfRates = new List<ExchangeRate>();

        try
        {
            //create http request and get the response from CNB
            using (HttpClient client = new HttpClient())
            {
                string requestUri = string.Format(URLs.CNB_URL_EXCHANGE_RATE, submittedDate);

                using HttpResponseMessage response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                responseBody = await response.Content.ReadAsStringAsync();
            }

            if (string.IsNullOrEmpty(responseBody))
            {
                _logger.LogWarning($"{logHeader} Data was not returned in response body");
                return listOfRates;
            }

            //processing the response and converting to submodel
            listOfRates = ExchangeRate.ConvertDataToExchangeRate(responseBody, currenciesArray);

            if (listOfRates == null || listOfRates.Count == 0)
            {
                _logger.LogWarning($"{logHeader} Empty list of exchange rates. Searched by date: {submittedDate}");
                return listOfRates;
            }

            TimeSpan endTime = DateTime.Now - startTime;

            _logger.LogInformation($"{logHeader} Data found, duration: {endTime}");
            return listOfRates;
        }

        catch (Exception ex)
        {
            _logger.LogError($"{logHeader} {ex.Message}");
            return listOfRates;
        }
    }
}
