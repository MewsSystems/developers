using ExchangeRateProvider.Contracts;

namespace ExchangeRateProviderCzechNationalBank.Interface
{
    public  interface IStoreExchangeRates
    {
        public DateTime RatesUpdatedOn { get; }

        /// <inheritdoc cref="StoreExchangeRates.GetRates(IEnumerable{Currency})"
        List<ExchangeRate> GetRates(IEnumerable<Currency> requestedCurrencies);

        /// <inheritdoc cref="StoreExchangeRates.UpdateRates(IList{ExchangeRate}, DateTime)"/>
        void UpdateRates(IList<ExchangeRate> ratesToUpdate, DateTime ratesUpdatedAt);

        /// <inheritdoc cref="StoreExchangeRates.UpdateRates(IList{ExchangeRate})"/>
        void UpdateRates(IList<ExchangeRate> ratesToUpdate);
    }
}
