using CurrencyModel = Mews.ExchangeRateUpdater.Services.Models.CurrencyModel;
using ExchangeRateModel = Mews.ExchangeRateUpdater.Services.Models.ExchangeRateModel;

namespace Mews.ExchangeRateUpdater.Services.ExternalRateProviders.CNB.Mapping
{
    public static class ExchangeRatesMapper
    {
        public static IEnumerable<ExchangeRateModel>? ToExchangeRateModels(this ExchangeRates exchangeRatesDto)
        {
            if (exchangeRatesDto == null) return null;

            var exchangeRateDetailsDtoCollection = exchangeRatesDto.Rates;

            var exchangeRateModelCollection = new List<ExchangeRateModel>();

            if(exchangeRateDetailsDtoCollection != null && exchangeRateDetailsDtoCollection.Any()) 
            {
                foreach (var exchangeRateDetailsDto in exchangeRateDetailsDtoCollection)
                {
                    var exchangeRateModel = ToExchangeRateModel(exchangeRateDetailsDto);
                    exchangeRateModelCollection.Add(exchangeRateModel);
                }
            }

            return exchangeRateModelCollection;
        }

        private static ExchangeRateModel ToExchangeRateModel(ExchangeRateDetails exchangeRateDetailsDto)
        {
            return new ExchangeRateModel
            {
                ValidFor = exchangeRateDetailsDto.ValidFor,
                Country = exchangeRateDetailsDto.Country,
                SourceCurrency = new CurrencyModel { Currency = exchangeRateDetailsDto.Currency, Code = exchangeRateDetailsDto.CurrencyCode },
                Amount = exchangeRateDetailsDto.Amount,
                Rate = exchangeRateDetailsDto.Rate
            };
        }
    }
}   


