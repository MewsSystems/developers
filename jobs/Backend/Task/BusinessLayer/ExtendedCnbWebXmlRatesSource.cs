using System.Linq;
using System.Threading.Tasks;
using ExchangeRateUpdater.Entities.Xml;

namespace ExchangeRateUpdater.BusinessLayer
{

    /// <summary>
    /// Source for combined list of daily basic currencies list and monthly exotic currencies list
    /// </summary>
    public class ExtendedCnbWebXmlRatesSource : CnbWebXmlRatesSource
    {
        protected string XmlUrlConfigKey => "CnbExoticRatesXMlUrl";

        public override async Task<CnbXmlRatesDocument> GetRates()
        {
            var result = await base.GetRates();
            var extended = await GetRates(GetXmlUrl(XmlUrlConfigKey));
            result.RatesTable.Rates = result.RatesTable.Rates.Concat(extended.RatesTable.Rates).ToArray();
            return result;
            
        }
    }
}
