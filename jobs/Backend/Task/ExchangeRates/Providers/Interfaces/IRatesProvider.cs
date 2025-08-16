using System.Runtime.CompilerServices;
using ExchangeRatesService.Models;

namespace ExchangeRatesService.Providers.Interfaces;

using System.Collections.Generic;

public interface IRatesProvider
{
    IAsyncEnumerable<ExchangeRate> GetRatesAsync(IEnumerable<Currency> currencies,
        [EnumeratorCancellation] CancellationToken cancellationToken = default);

    IAsyncEnumerable<ExchangeRate> GetRatesReverseAsync(IEnumerable<Currency> currencies, decimal amount,
        [EnumeratorCancellation] CancellationToken cancellationToken = default);
}
