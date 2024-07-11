using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.RateProvider;

namespace ExchangeRateUpdater;

public class TestService(ExchangeRateProvider provider)
{
    private readonly ExchangeRateProvider _provider = provider;
    private readonly static IEnumerable<Currency> currencies = [
        new Currency("USD"),
        new Currency("EUR"),
        new Currency("CZK"),
        new Currency("JPY"),
        new Currency("KES"),
        new Currency("RUB"),
        new Currency("THB"),
        new Currency("TRY"),
        new Currency("XYZ")
    ];

    public async Task ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var rates = await _provider.GetExchangeRates(currencies, cancellationToken);
        Console.WriteLine($"Successfully retrieved {rates.Count()} exchange rates:");
        foreach (var rate in rates)
        {
            Console.WriteLine(rate.ToString());
        }
        Console.ReadLine();
    }
}
