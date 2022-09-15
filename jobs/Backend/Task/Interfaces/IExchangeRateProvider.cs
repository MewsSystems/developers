using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateProvider
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies, string sourceUrl);
        Task<XDocument> GetXmlSource(string sourceUrl);
        string GetXmlAttribute(string elementName, XElement item);
    }
}