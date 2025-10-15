using System.Collections.Generic;

namespace ExchangeRateUpdater.Services;

public interface ICnbParser
{
    IEnumerable<(string Code, int Amount, decimal Rate)> Parse(string payload);
}
