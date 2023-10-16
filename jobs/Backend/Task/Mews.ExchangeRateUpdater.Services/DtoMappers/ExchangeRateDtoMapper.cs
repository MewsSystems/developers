using Mews.ExchangeRateUpdater.Dtos;
using Mews.ExchangeRateUpdater.Services.Models;

namespace Mews.ExchangeRateUpdater.Services.DtoMappers
{
    /// <summary>
    /// This is the mapper which is used to map ExchangeRateModel collection to ExchangeRateDto collection
    /// </summary>
    public static class ExchangeRateDtoMapper
    {
        public static IEnumerable<ExchangeRateDto> ToExchangeRateDtos(this IEnumerable<ExchangeRateModel> exchangeRateModelCollection) 
        {
            if(exchangeRateModelCollection == null) return Enumerable.Empty<ExchangeRateDto>();

            var exchangeRateDtoCollection = new List<ExchangeRateDto>();

            foreach( var exchangeRateModel in exchangeRateModelCollection)
            {
                var exchangeRateDto = ToExchangeRateDto(exchangeRateModel);
                exchangeRateDtoCollection.Add(exchangeRateDto);
            }

            return exchangeRateDtoCollection;
        }

        private static ExchangeRateDto ToExchangeRateDto(ExchangeRateModel exchangeRateModel)
        {
            var sourceCurrencyDto = exchangeRateModel.SourceCurrency.ToCurrencyDto();
            var targetCurrencyDto = exchangeRateModel.TargetCurrency.ToCurrencyDto();

            // Here we are dividing by Amount to calculate the exchange rate for the single amount of currency, as sometimes
            // the external provider might respond with more than 1 as a value for the Amount
            // Example: 100 INR/CZK=28.15, so INR/CZK = 28.15 / 100 = 0.28
            var exchangeRate = exchangeRateModel.Rate / exchangeRateModel.Amount;

            return new ExchangeRateDto(sourceCurrencyDto, targetCurrencyDto, exchangeRate);
        }
    }
}
