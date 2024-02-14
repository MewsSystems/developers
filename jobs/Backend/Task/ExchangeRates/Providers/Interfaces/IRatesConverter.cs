using ExchangeRatesService.Models;

namespace ExchangeRatesService.Providers.Interfaces;

public interface IRatesConverter
{
    public Task<decimal> GetConversion(Currency source, Currency target, decimal rate, decimal amount);
}