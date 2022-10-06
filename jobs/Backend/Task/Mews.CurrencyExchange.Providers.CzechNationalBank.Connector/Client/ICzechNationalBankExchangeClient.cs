namespace Mews.CurrencyExchange.Providers.CzechNationalBank.Connector.Client
{
    internal interface ICzechNationalBankExchangeClient
    {
        Task<IEnumerable<ProviderExchangeRate>> FetchExchangeRates();
    }
}
