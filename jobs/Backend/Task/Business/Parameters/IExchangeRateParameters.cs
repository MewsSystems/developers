using System;

namespace ExchangeRateUpdater.Business.Parameters
{
    internal interface IExchangeRateParameters
    {
        string BaseURL { get; set; }
        string ExchangeRatesDailyEndpoint { get; set; }
        DateTime Date { get; set; }
        string Language { get; set; }
    }
}