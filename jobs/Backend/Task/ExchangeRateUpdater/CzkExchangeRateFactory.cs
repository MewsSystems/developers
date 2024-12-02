namespace ExchangeRateUpdater;

internal class CzkExchangeRateFactory {
    public ExchangeRate Create(ExchangeRateProvider.ExchangeRateResponse rateResponse)
    {
        return new ExchangeRate (new Currency(ExchangeRateProvider.CZECH_KORUNA_CODE),
                                 new Currency(rateResponse.CurrencyCode),
                                 rateResponse.Rate / rateResponse.Amount);
    }
}

