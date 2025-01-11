using ExchangeRateUpdater.DTOs;

namespace ExchangeRateUpdater.Services;

public class ExchangeRateService
{
    public ExchangeRatesDTO GetExchangeRates()
    {
        return new ExchangeRatesDTO();
    }
}
