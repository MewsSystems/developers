using System;

namespace ExchangeRateUpdater.Business.Parameters
{
    /**
     * This class represents a set of possible parameterised details regarding the exchange rate for the Czech National Bank.
     * These could be:
     *      - viewed/updated from a frontend parameter page 
     *      - retrieved from a database with an entity/repository pattern before using them in the service class that performs the API call.
     */
    internal class ExchangeRateParameters : IExchangeRateParameters
    {
        public string BaseURL { get; set; } = "https://api.cnb.cz/cnbapi/";
        public string ExchangeRatesDailyEndpoint { get; set; } = "exrates/daily";
        public DateTime Date { get; set; } = DateTime.Now;
        public string Language { get; set; } = "EN";
    }
}
