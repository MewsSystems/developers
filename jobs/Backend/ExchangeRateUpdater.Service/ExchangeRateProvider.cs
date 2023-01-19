using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Service
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _cnbExchangeRatesUrl;        

        private const string CnbExchangeRatesUrlSettingsKey = "cnbExchangeRatesUrl";

        public ExchangeRateProvider(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _cnbExchangeRatesUrl = configuration.GetValue<string>(CnbExchangeRatesUrlSettingsKey);
            if(_cnbExchangeRatesUrl == null ) 
            {
                throw new ConfigurationErrorsException($"Required key not found on configuration context: {CnbExchangeRatesUrlSettingsKey}");
            }
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, DateTime? referenceDate = null)
        {
            if (!referenceDate.HasValue)
            {
                referenceDate = DateTime.Today;
            }

            var client = _httpClientFactory.CreateClient("HttpClient");

            var urlRequest = string.Format($"{_cnbExchangeRatesUrl}?date={referenceDate.Value.ToString("dd.MM.yyyy")}");

            var exchangeRatesResponse = await client.GetStreamAsync(urlRequest);

            XmlSerializer serializer = new XmlSerializer(typeof(kurzy));

            kurzy exchangeRates = (kurzy)serializer.Deserialize(exchangeRatesResponse);

            var czCulture = new CultureInfo("cs");

            return exchangeRates.tabulka.radek
                .Where(x => currencies.Select(y => y.Code)
                .Contains(x.kod, StringComparer.InvariantCultureIgnoreCase))
                .Select(x => new ExchangeRate(
                    new Currency(x.kod),
                    new Currency("CZK"), //Fixing target currency as CZK since the soure is limited
                    decimal.Parse(x.kurz, czCulture) / x.mnozstvi));
        }
    }
}
