using ExchangeRateUpdater.Clients.Cnb.Responses;

namespace ExchangeRateUpdater.Tests.Shared.Builders;

public class ExchangeRateResponseBuilder
{
    private DateTime _currentDate = DateTime.Now;

    private List<ExchangeRateDto?> _exchangeRates = new();

    public ExchangeRateResponse Build()
    {
        return new ExchangeRateResponse
        {
            CurrentDate = _currentDate,
            ExchangeRates = _exchangeRates
        };
    }

    public ExchangeRateResponseBuilder WithCurrentDate(DateTime dateTime)
    {
        _currentDate = dateTime;
        return this;
    }

    public ExchangeRateResponseBuilder WithExchangeRates(List<ExchangeRateDto?> exchangeRates)
    {
        _exchangeRates = exchangeRates;
        return this;
    }
}