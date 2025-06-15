using ExchangeRateUpdater.Exchanges.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace ExchangeRateUpdater.Exchanges;

public static class ExchangeFactory
{
    /// <summary>
    /// Creates an exchange provider based on the "PROVIDER" setting that was chosen
    /// </summary>
    /// <param name="config">Configuration instance with the provider set.</param>
    /// <param name="httpResilientClient">HttpResilientClient instance to be used as wrapper for http calls.</param>
    /// <param name="logger">Logger instance.</param>
    /// <returns>Returns an instance of the selected ExchangeRateProvider</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static IExchangeRateProvider GetExchangeRateProvider(IConfiguration config, IHttpResilientClient httpResilientClient, ILogger logger)
    {
        var provider = config["PROVIDER"];

        if (string.IsNullOrEmpty(provider))
        {
            var errMissingVar = "Exchange rate provider not set";
            logger.LogCritical(errMissingVar);
            throw new InvalidOperationException(errMissingVar);
        }

        if(provider == "CNB")
        {
            return new CnbExchangeRateProvider(config, httpResilientClient, logger);
        }

        var errWrongProvider = "Invalid exchange rate provider: " + provider;
        logger.LogCritical(errWrongProvider);
        throw new InvalidOperationException(errWrongProvider);
    }
}
