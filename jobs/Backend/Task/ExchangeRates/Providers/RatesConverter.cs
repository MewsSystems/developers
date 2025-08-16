using ExchangeRatesService.Models;

namespace ExchangeRatesService.Providers.Interfaces;

public class RatesConverter: IRatesConverter, IAsyncDisposable
{
    public async Task<decimal> GetConversion(Currency source, Currency target, decimal rate, decimal amount)
    {
        return await Task.Run(() => amount * rate);
    }

    public ValueTask DisposeAsync()
    {
        Console.WriteLine($"{nameof(RatesConverter)}.Dispose()");
        return ValueTask.CompletedTask;
    }
}