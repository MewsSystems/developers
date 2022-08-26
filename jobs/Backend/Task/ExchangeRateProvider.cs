using ExchangeRateUpdater.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater;

public class ExchangeRateProvider
{
    //TODO: add ctr and inject in AppSettings... overkill?

    /// <summary>
    /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
    /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
    /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
    /// some of the currencies, ignore them.
    /// </summary>
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
    {
        var client = new HttpClient();//TODO: inject as a named HttpClient if we used DI
        var result = await client.GetStringAsync(
            "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");//TODO: store URL in appsettings.json

        var xmlObject = DeserializeToObject<kurzy>(result);
        var l = new List<ExchangeRate>();
        foreach (var line in xmlObject.tabulka.radek)
        {
            var targetCurrency = new Currency(line.kod);
            if (currencies.Count(p => p.Code == targetCurrency.Code) == 1)//TODO: dictionary lookup is faster
            {
                //TODO: store base currency code CZK in appsettings
                var toRate = new ExchangeRate(new Currency("CZK"), targetCurrency, line.kurzUseable);
                l.Add(toRate);
            }
        }
        return l;
    }

    //TODO: store/reuse via helper class/library
    T DeserializeToObject<T>(string input) where T : class
    {
        var ser = new System.Xml.Serialization.XmlSerializer(typeof(T));
        using (var reader = new StringReader(input))
        return (T)ser.Deserialize(reader);
    }
}
