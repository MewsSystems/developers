namespace ExchangeRateUpdater;

public class AppRoot
{
    private readonly ExchangeRateProvider _exchangeRateProvider;
    
    public AppRoot(ExchangeRateProvider exchangeRateProvider)
    {
        _exchangeRateProvider = exchangeRateProvider;
    }
    
    private IEnumerable<Currency> currencies = new[]
    {
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    };
    
    public async Task RunAsync()
    {
        var rates = await _exchangeRateProvider.GetExchangeRatesAsync(currencies);

        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        
        foreach (var rate in rates)
        {
            Console.WriteLine(rate.ToString());
        }
    }
}