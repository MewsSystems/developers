using ExchangeRateUpdater.Application;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Infrastructure
{
    public class ExchangeRateProvider
    {
        private static readonly Currency TargetCurrency = new("CZK");
        private readonly ICzechNationalBankClient _source;
        private readonly ILogger<ExchangeRateProvider> _logger;
        private readonly IValidator<CnbExchangeRate> _validator;

        public ExchangeRateProvider(
            ICzechNationalBankClient source,
            ILogger<ExchangeRateProvider> logger,
            IValidator<CnbExchangeRate> validator)
        {
            _source = source;
            _logger = logger;
            _validator = validator;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(
            IEnumerable<Currency> currencies, CancellationToken cancellationToken)
        {
            var exchangeRates = new List<ExchangeRate>();

            var sourceExchangeRates = (await _source.GetExchangeRatesAsync(cancellationToken))
                .Rates
                .ToDictionary(rate => rate.CurrencyCode);

            foreach (var code in currencies.Select(currency => currency.Code))
            {
                var sourceExchangeRate = sourceExchangeRates.GetValueOrDefault(code);
                if (sourceExchangeRate == null)
                {
                    _logger.LogInformation("Could not find currency {currency}", code);
                    continue;
                }

                var validationResult = _validator.Validate(sourceExchangeRate);
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Skipping currency {currency} due to validation errors: {errors}",
                        code, validationResult.ToString());

                    continue;
                }

                var exchangeRate = new ExchangeRate(
                    SourceCurrency: new Currency(sourceExchangeRate.CurrencyCode),
                    TargetCurrency: TargetCurrency,
                    Value: sourceExchangeRate.Rate / sourceExchangeRate.Amount);

                exchangeRates.Add(exchangeRate);
            }

            return exchangeRates;
        }
    }
}
