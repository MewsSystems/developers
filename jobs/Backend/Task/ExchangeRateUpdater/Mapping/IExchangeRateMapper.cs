using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Mapping;

public interface IExchangeRateMapper<T>
{
    /// <summary>
    /// Maps the provided type <see cref="T"/> to an <see cref="ExchangeRate"/>
    /// </summary>
    /// <param name="typeToConvert"><see cref="T"/></param>
    /// <returns>Mapped <see cref="ExchangeRate"/></returns>
    ExchangeRate MapToExchangeRate(T typeToConvert);
}