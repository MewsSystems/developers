namespace ExchangeRateUpdater.Code.Application
{
    using System.Collections.Generic;
    using System.Linq;
    using ExchangeRateUpdater.Code.Observability;
    using ExchangeRateUpdater.Data;
    using ExchangeRateUpdater.Domain;

    public class ExchangeRateProvider
    {
        private readonly ILogger logger;
        private readonly IExchangeRateData echangeRateData;
        private readonly Currency targetCurrency;

        public ExchangeRateProvider(ILogger logger, IExchangeRateData echangeRateData, Currency targetCurrency)
        {
            this.targetCurrency = targetCurrency;
            this.logger = logger;
            this.echangeRateData = echangeRateData;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var exData = echangeRateData
                .GetExchangeRateData();

            logger.LogInformation($"Successfully retrieved exchange date from {exData.Bank}");

            return currencies
                .Where(c => exData.BankExchangeRateLink.BankExchangeRateData.Any(d => d.Code == c.Code))
                .Select(c =>
                {
                    var exchangeData = exData
                        .BankExchangeRateLink
                        .BankExchangeRateData
                        .FirstOrDefault(d => d.Code == c.Code);

                    return new ExchangeRate(new Currency(exchangeData.Code), targetCurrency, exchangeData.CalculatedRate);
                });
        }
    }
}
