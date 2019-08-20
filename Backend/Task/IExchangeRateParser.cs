using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater {
    internal interface IExchangeRateParser {
        Task<IEnumerable<ExchangeRate>> ParseAsync(Stream stream);
    }
}