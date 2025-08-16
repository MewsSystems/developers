using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRateUpdater.Model;

namespace ExchangeRateUpdater
{
    /// <summary>
    /// Client for REST API provided by Czech National Bank (CNB) 
    /// </summary>
    /// <seealso cref="https://api.cnb.cz/cnbapi/swagger-ui.html"/>
    public interface ICnbApiClient
    {
        /// <summary>
        /// Gets daily fixed Exchage rates for the current date. 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        Task<ICollection<ExchangeRate>> GetDailyRates(CancellationToken cancellationToken);
    }
}