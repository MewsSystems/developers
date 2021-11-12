using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRateDto>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            string response = await ApiHelper.ConsumeEndpoint("https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");
            if (response == null){throw new ArgumentNullException(nameof(response));}
            var deserialized = XmlHelper.Deserialize<ExchangeRate>(response);
            var exchangeRates = deserialized.Table.Rows.Where(b => currencies.Any(a => b.Code.Contains(a.Code))).ToList();
            var exchangeRatesDto = exchangeRates.Select(a => new ExchangeRateDto(new Currency(a.Code), new Currency("CZK"), Convert.ToDecimal(a.ExchangeRate))).ToList();

            return exchangeRatesDto;
        }
    }
}
