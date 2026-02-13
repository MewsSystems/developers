using ExchangeRateAPI.Models;
using ExchangeRateUpdater.Business;

namespace ExchangeRateAPI.Adapters
{
    public static class ExchangeRateAdapter
    {
        public static List<Currency> ConvertToCurrencyList(List<string> currencyCodes)
            => currencyCodes.Select(currencyCode => new Currency(currencyCode)).ToList();

        private static ExchangeRateResponse ToExchangeRateResponse(this ExchangeRate exchangeRate)
        {
            return new ExchangeRateResponse
            {
                SourceCurrency = exchangeRate.SourceCurrency.Code,
                TargetCurrency = exchangeRate.TargetCurrency.Code,
                Value = exchangeRate.Value
            };
        }

        public static List<ExchangeRateResponse> ToExchangeRateResponses(this List<ExchangeRate> exchangeRates)
            => exchangeRates.Select(exchangeRate => exchangeRate.ToExchangeRateResponse()).ToList();

    }
}
