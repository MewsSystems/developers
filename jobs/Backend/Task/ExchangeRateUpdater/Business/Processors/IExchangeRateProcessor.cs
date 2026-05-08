using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Business.Processors;

public interface IExchangeRateProcessor
{
    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// </summary>
    /// <param name="currenciesToProcess"></param>
    /// <returns>List of <see cref="ExchangeRate"/></returns>
    Task<Result<IEnumerable<ExchangeRate>>> ProcessExchangeRates(string[] currenciesToProcess);
}