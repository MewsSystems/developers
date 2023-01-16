using ExchangeRateUpdater.WebApi.Services.ExchangeRateParser;

namespace ExchangeRateUpdater.WebApi.Services.ExchangeRateProvider;

public class ExchangeRateProvider : IExchangeRateProvider
{
    private readonly IExchangeRateParser _exchangeRateParser;

    public ExchangeRateProvider(IExchangeRateParser exchangeRateParser)
    {
        _exchangeRateParser = exchangeRateParser;
    }

    public async Task<ServiceResponse<IEnumerable<ExchangeRate>>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var usedExchangeRates = Array.Empty<ExchangeRate>();

        var inputCurrencies = currencies.ToArray();

        if (!inputCurrencies.Any())
        {
            return new ServiceResponse<IEnumerable<ExchangeRate>>
            {
                Data = usedExchangeRates,
                Success = true,
                Message = $"Successfully retrieved {usedExchangeRates.Length} exchange rates."
            };
        }

        IEnumerable<ExchangeRate> availableExchangeRates = await _exchangeRateParser.ParseExchangeRates();

        usedExchangeRates = availableExchangeRates
            .Where(exchangeRate =>
                inputCurrencies.Any(currency => string.Equals(exchangeRate.TargetCurrency.Code, currency.Code,
                    StringComparison.OrdinalIgnoreCase)))
            .ToArray();

        return new ServiceResponse<IEnumerable<ExchangeRate>>
        {
            Data = usedExchangeRates,
            Success = true,
            Message = $"Successfully retrieved {usedExchangeRates.Length} exchange rates."
        };
    }
}