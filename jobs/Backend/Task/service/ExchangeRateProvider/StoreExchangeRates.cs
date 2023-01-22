using ExchangeRateProvider.Contracts;
using ExchangeRateProviderCzechNationalBank.Interface;

namespace ExchangeRateProviderCzechNationalBank
{
    public class StoreExchangeRates : IStoreExchangeRates
    {
        private List<ExchangeRate> rates = new List<ExchangeRate>();
        public DateTime RatesUpdatedOn { get; private set; } = DateTime.MinValue;

        /// <param name="requestedCurrencies">Is expected to NOT be null</param>
        public List<ExchangeRate> GetRates(IEnumerable<Currency> requestedCurrencies)
        {
            var requestedCurrenciesCodes = requestedCurrencies.Select(x => x.Code).ToList();
            //We don't want to have any references to stored objects.
            var ratesToReturn = rates.Where(x => requestedCurrenciesCodes.Contains(x.SourceCurrency.Code))
                .Select(x => new ExchangeRate(x.SourceCurrency, x.TargetCurrency, x.Value)).ToList();
            return ratesToReturn;
        }

        /// <summary>Updates rates and sets dateTime of the update to prevent another calls before the next rateChange</summary>
        public void UpdateRates(IList<ExchangeRate> ratesToUpdate, DateTime ratesUpdatedOn)
        {
            if(ratesToUpdate != null && ratesToUpdate.Count > 0)
            {
                //We don't want to have any references to stored objects.
                var newRates = ratesToUpdate.Select(x => new ExchangeRate(x.SourceCurrency, x.TargetCurrency, x.Value)).ToList();
                rates = newRates;
                RatesUpdatedOn = ratesUpdatedOn;
            }
        }

        /// <summary>Use this in case of some of the source failure</summary>
        public void UpdateRates(IList<ExchangeRate> ratesToUpdate)
        {
            //We don't want to have any references to stored objects.
            var newRates = ratesToUpdate.Select(x => new ExchangeRate(x.SourceCurrency, x.TargetCurrency, x.Value)).ToList();
            rates = newRates;
        }

        #region testPurposes
        //Honestly I am not sure if this is right approche. I am used to do unit tests lightly. So only available public methods
        /// <summary>This method is only for testing, You should always access this service throught interface and DI!</summary>
        public List<ExchangeRate> TestGetRates()
        {
            return rates;
        }
        #endregion
    }
}
