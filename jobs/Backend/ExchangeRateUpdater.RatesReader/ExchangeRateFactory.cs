using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.RatesReader
{
    public static class ExchangeRateFactory
    {
        //In this approache we are keeping all the validation in the domain and avoiding having it in the factory class wich, depending on the entity can become big and hard to navigate
        //We are also fallowign the Single responsability principle by not having multiple reasons to go and modify the factory
        public static Result<CurrencyExchangeRate> CreateExchangeRateFromCZK(ExchangeRateReadModel model)
        {
            var sourceCurrencyResult = Currency.Create("CZK");
            var destinationCurrencyResult = Currency.Create(model.CurrencyCode);
            var exchangeRateResult = ExchangeRate.Create(model.Amount, model.Rate);
            var combinedResult = Result.Combine(sourceCurrencyResult, destinationCurrencyResult, exchangeRateResult);
            return combinedResult.Succsess ?
                Result.OK(new CurrencyExchangeRate(sourceCurrencyResult.Value, destinationCurrencyResult.Value, exchangeRateResult.Value)) :
                Result.Fail<CurrencyExchangeRate>(combinedResult.FailureResons);
        }
    }
}