using System.Threading.Tasks;
using ExchangeRateUpdater.Entities.Xml;

namespace ExchangeRateUpdater.Interfaces
{

    /// <summary>
    /// Interface to provide access to the parsed CNB XML Rates format 
    /// </summary>
    public interface ICnbXmlSource
    {
        Task<CnbXmlRatesDocument> GetRates();
    }
}
