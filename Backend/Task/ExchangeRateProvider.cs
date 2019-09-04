using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

namespace ExchangeRateUpdater
{
    public class ExchangeRateProvider
    {
        
        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "EUR/USD" but not "USD/EUR",
        /// do not return exchange rate "USD/EUR" with value calculated as 1 / "EUR/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            var culture = CultureInfo.CreateSpecificCulture("cs-CZ");

            using (XmlTextReader reader = new XmlTextReader(Consts.ServiceUrl))
            {
                var requestedCurrencies = currencies.Select(c => c.Code).ToArray();

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name.Equals(Consts.XmlElementName))
                    {
                        var currentCode = reader[Consts.XmlAtrributeCode];
                        decimal currentValue = 0;
                        decimal multiplyValue = 0;

                        if (requestedCurrencies.Contains(currentCode))
                        {
                            if(decimal.TryParse(reader[Consts.XmlAtrributeExchRate], NumberStyles.Any, culture, out currentValue) && decimal.TryParse(reader[Consts.XmlAtrributeQuantity], NumberStyles.Any, culture, out multiplyValue))
                            {
                                yield return new ExchangeRate(new Currency(Consts.SourceCurrency), new Currency(currentCode), currentValue / multiplyValue);
                            }
                        }
                    }
                }
            }

        }
    }
}
