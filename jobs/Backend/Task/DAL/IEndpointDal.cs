using System.Collections.Generic;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.DAL;

/// <summary>
/// Data access layer to acquire exchange rate endpoints
/// </summary>
public interface IEndpointDal
{
    /// <summary>
    /// Retrieve all bank data endpoints
    /// </summary>
    /// <returns></returns>
    IEnumerable<IExchangeRateEndpoint> LoadEndpoints();
}