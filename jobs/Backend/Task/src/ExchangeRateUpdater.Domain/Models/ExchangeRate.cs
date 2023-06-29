namespace ExchangeRateUpdater.Domain.Models;

public record ExchangeRate(CurrencyPair CurrencyPair, decimal Value)
{
    public override string ToString() => $"{CurrencyPair}={Value}";
}

