using ExchangeRateUpdater.ApiClient.Client.ExchangeDaily;
using ExchangeRateUpdater.Models.Domain;

namespace ExchangeRateUpdater.Features.Services.ExchangeRagesDaily.V1
{
    public static class Mapper
    {
        public static IEnumerable<Currency> ToCurrency(this IEnumerable<CurrencyModel> models)
        {
            foreach (var item in models)
                yield return item.ToCurrency();
        }

        public static Currency ToCurrency(this CurrencyModel currencyModel)
        {
            return new Currency(currencyModel.Code);
        }

        public static IEnumerable<ExchangeRateModel> ToExchangeRateModel(this IEnumerable<ExchangeRate> exhangeRates)
        {
            foreach (var item in exhangeRates)
            {
                yield return new ExchangeRateModel()
                {
                    SourceCurrency = item.SourceCurrency.ToCurrencyModel(),
                    TargetCurrency = item.TargetCurrency.ToCurrencyModel(),
                    Value = item.Value

                };
            }
        }

        public static CurrencyModel ToCurrencyModel(this Currency currency)
        {
            return new CurrencyModel()
            {
                Code = currency.Code
            };
        }

        public static IEnumerable<ExchangeRate> ToExchangeRate(this IEnumerable<ExchangeRateDailyResponse> models, Currency sourceCurrency)
        {
            foreach (var item in models)
                yield return new ExchangeRate(sourceCurrency,
                    new Currency(item.CurrencyCode.ToUpperInvariant()),
                    item.Rate);
        }
    }
}
