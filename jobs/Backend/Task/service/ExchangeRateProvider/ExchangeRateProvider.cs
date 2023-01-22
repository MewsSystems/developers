using ErrorOr;
using ExchangeRateProvider.Contracts;
using ExchangeRateProviderCzechNationalBank.CNBResponseModel;
using ExchangeRateProviderCzechNationalBank.Interface;
using ExchangeRateProviderCzechNationalBank.Logging;
using Microsoft.Extensions.Logging;
using System.Xml.Serialization;

namespace ExchangeRateProviderCzechNationalBank
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly IStoreExchangeRates _exchangeRatesStorage;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(HttpClient httpClient, IStoreExchangeRates exchangeRatesStorage, ILogger<ExchangeRateProvider> logger)
        {
            _httpClient = httpClient;
            _exchangeRatesStorage = exchangeRatesStorage;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var result = Enumerable.Empty<ExchangeRate>();

            if(currencies != null && currencies.Count() > 0)
            {
                await CheckForRatesUpdate();
                result = _exchangeRatesStorage.GetRates(currencies);
            }

            return result;
        }

        private async Task CheckForRatesUpdate()
        {
            var now = DateTime.UtcNow;
            if (UpdateExchangeRatesConditions(now))
            {
                await UpdateRates(now);
            }
        }

        //If you want to be double sure, we can check ČNB response for change in Kurzy.Order
        public bool UpdateExchangeRatesConditions(DateTime now)
        {
            //ČNB changes rates every work day after 13:30 UTC.
            //If we make too many requests they will start to block us.
            //Hence it would be best to update the rates once a day.
            //https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu
            //https://www.cnb.cz/cs/casto-kladene-dotazy/Kurzy-devizoveho-trhu-na-www-strankach-CNB/
            var rateChange = new DateTime(now.Year, now.Month, now.Day, 13, 31, 0);
            var firstRequestAfterTodaysRateChange = now > rateChange && rateChange > _exchangeRatesStorage.RatesUpdatedOn;
            var firstRequestSinceYesterdaysRateChange = now < rateChange && rateChange.AddDays(-1) > _exchangeRatesStorage.RatesUpdatedOn;
            return firstRequestAfterTodaysRateChange || firstRequestSinceYesterdaysRateChange;
        }

        private async Task UpdateRates(DateTime consideredTimeOfUpdate)
        {
            var newRatesResult = await GetRatesFromBank("financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");
            var otherCurrenciesRatesResult = await GetRatesFromBank("financni-trhy/devizovy-trh/kurzy-ostatnich-men/kurzy-ostatnich-men/kurzy.xml");

            var isAnyResultError = newRatesResult.IsError || otherCurrenciesRatesResult.IsError;
            var areBothResultsError = newRatesResult.IsError && otherCurrenciesRatesResult.IsError;
            if (isAnyResultError && !areBothResultsError)
            {
                //We were unable to receive whole data from the source.
                //So we will update the store with actual data but not prevent us from calling the source again.
                newRatesResult.Switch(
                    newRates => _exchangeRatesStorage.UpdateRates(newRates),
                    errors => _exchangeRatesStorage.UpdateRates(otherCurrenciesRatesResult.Value));
            }
            else if(!isAnyResultError)
            {
                newRatesResult.Value.AddRange(otherCurrenciesRatesResult.Value);
                _exchangeRatesStorage.UpdateRates(newRatesResult.Value, consideredTimeOfUpdate);
            }
        }

        private async Task<ErrorOr<List<ExchangeRate>>> GetRatesFromBank(string specificAddress)
        {
            var result = new List<ExchangeRate>();
            try
            {
                var response = await _httpClient.GetStreamAsync(specificAddress);
                XmlSerializer serializer = new XmlSerializer(typeof(Kurzy));
                Kurzy exchangeRates = (Kurzy)serializer.Deserialize(response);
                result = exchangeRates.Table.Line.Select(x => 
                    new ExchangeRate(
                        new Currency(x.Code), 
                        new Currency("CZK"), 
                        decimal.Parse(x.Rate) / int.Parse(x.Amount))).ToList();
            }
            catch(Exception ex) 
            {
                _logger.LogCzechNationalBankResponseFailure(_httpClient.BaseAddress != null ? _httpClient.BaseAddress.ToString() + specificAddress : string.Empty, ex);
                return Error.Failure();
            }
            return result;
        }

    }
}
