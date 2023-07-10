using ExchangeRateUpdater.CnbProvider.CnbClientResponses;
using ExchangeRateUpdater.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.CnbProvider.Abstractions
{
    public interface ICnbRateProviderClient
    {
        /// <summary>
        /// Call to Cnb API based on the url parameter
        /// </summary>
        /// <param name="url"></param>
        /// <returns>A collection of Rates provided by Cnb</returns>
        Task<IEnumerable<CnbRateResponseDto>> GetRatesByDateAsync(string url);
    }
}
