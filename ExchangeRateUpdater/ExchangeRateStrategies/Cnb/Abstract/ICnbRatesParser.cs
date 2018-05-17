using ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Model;
using System.Collections.Generic;

namespace ExchangeRateUpdater.ExchangeRateStrategies.Cnb.Abstract
{
    public interface ICnbRatesParser
    {
        /// <summary>
        /// Parser provided string contents of txt file
        /// </summary>
        /// <param name="contents">Contents of txt file</param>
        /// <returns>List of rates</returns>
        IEnumerable<CnbRate> Parse(string contents);
    }
}
