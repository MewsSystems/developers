using ExchangeRateUpdater.DTOs;
using System.Net.Http;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService
{
    public ExchangeRatesDTO GetExchangeRates()
    {
        return new ExchangeRatesDTO
        {
            Rates = new List<ExchangeRateDTO>
            {
                new ()
                {
                    ValidFor = "2025-01-11",
                    Order = 1,
                    Currency = "Currency",
                    Country = "Country",
                    Amount = 1,
                    CurrencyCode = "CUR",
                    Rate = 10.00M
                }
            }
        };
    }
}
