using ExchangeRateUpdater.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static ExchangeRateUpdater.Enums.Banks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        private readonly IApiService _apiService;
        private readonly IDataModifyingService _dataModifyingService;
        private readonly IBankCurrencyService _bankCurrencyService;

        public ExchangeRateProvider(IApiService apiService,
            IDataModifyingService dataModifyingService, IBankCurrencyService bankCurrencyService)
        {
            _apiService = apiService;
            _dataModifyingService = dataModifyingService;
            _bankCurrencyService = bankCurrencyService;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        /// 
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            try
            {
                var stringApiResult = await _apiService.GetApiDataAsStringAsync(Routes.ApiUri + Routes.GetDataRoute);
                var exchangeData = _dataModifyingService.DeserializeString(stringApiResult);

                var commonCurrencies = _dataModifyingService.CommonCurrencies(exchangeData.Data.Entities, currencies);

                BankType bankType;
                Enum.TryParse(exchangeData.Bank, out bankType);

                var bankCurrency = _bankCurrencyService.FindBankCurrency(bankType);

                var exchangeRates = new List<ExchangeRate>();
                foreach (var item in commonCurrencies)
                {
                    exchangeRates.Add(new ExchangeRate(new Currency(item.Code), bankCurrency, item.Value));
                }

                return exchangeRates;
            }
            catch (Exception)
            {
                //LOG
                return Enumerable.Empty<ExchangeRate>();
            }
        }
    }
}
