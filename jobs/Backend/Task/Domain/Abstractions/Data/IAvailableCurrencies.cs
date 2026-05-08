using Domain.Models;

namespace Domain.Abstractions.Data;

public interface IAvailableCurrencies
{
    IEnumerable<Currency> GetCurrencies();
    Currency GetCurrencyWithCode(string code);
}
