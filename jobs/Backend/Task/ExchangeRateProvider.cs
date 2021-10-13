using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.Data.Models;
using ExchangeRateUpdater.Utils;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        List<ExchangeRate> validExchangeRates = new List<ExchangeRate>();
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            CNBExchangeRate cNBExchangeRate = XmlDeserializer.DeserializeXMLFileToObject<CNBExchangeRate>(await CNBConnection.GetXml(CNBConnection.ExchangeRateXml));
            Currency czk = new Currency("CZK");

            //var match = currencies.Where(x => cNBExchangeRate.RatesTable.ExchangeRates.Select(c => c.Code).Contains(x.Code));

            foreach (Rate exchangeRate in cNBExchangeRate.RatesTable.ExchangeRates)
            {
                exchangeRate.ExchangeRate = exchangeRate.ExchangeRate.Replace(",", ".");
                decimal xmlExchangeRate = decimal.Parse(exchangeRate.ExchangeRate, CultureInfo.InvariantCulture);
                decimal value = xmlExchangeRate * exchangeRate.Amount;

                foreach (Currency allowedCurrency in currencies)
                {
                    if (allowedCurrency.Code == exchangeRate.Code)
                    {
                        ExchangeRate validRate = new ExchangeRate(czk, allowedCurrency, value);
                        validExchangeRates.Add(validRate);
                    }
                    else
                    {
                        _logger.Error($"{allowedCurrency.Code} was not found in source XML");

                    }
                }

            }

            return await Task.FromResult(validExchangeRates.ToList());
        }
    }
}
