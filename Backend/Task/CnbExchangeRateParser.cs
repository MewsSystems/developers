using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml;

namespace ExchangeRateUpdater
{
    class CnbExchangeRateParser : IExchangeRateParser
    {
        public IEnumerable<ExchangeRate> Parse()
        {
            // use this culture for parsing
            var cultureInfo = CultureInfo.CreateSpecificCulture(Properties.Settings.Default.CNB_XmlCultureCode);

            using (XmlReader xmlReader = XmlReader.Create(Properties.Settings.Default.CNB_XmlUrl))
            {
                // Parse all available exchange rates
                while (xmlReader.Read())
                {
                    // Find elements containing currency
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name.Equals(Properties.Settings.Default.CNB_XmlElementName))
                    {
                        var targetCurrencyCode = xmlReader[Properties.Settings.Default.CNB_XmlAttrCode];

                        decimal exchangeRate = 0m;
                        var exchangeRateValid = decimal.TryParse(xmlReader[Properties.Settings.Default.CNB_XmlAttrRate], NumberStyles.Any, cultureInfo, out exchangeRate);

                        decimal amount = 0m;
                        var amountValid = decimal.TryParse(xmlReader[Properties.Settings.Default.CNB_XmlAttrAmount], NumberStyles.Any, cultureInfo, out amount);

                        // check all required fileds are populated
                        if (!string.IsNullOrEmpty(targetCurrencyCode) && exchangeRateValid && amountValid && amount > 0m)
                        {
                            // Assume that CNB source currency is CZK
                            var sourceCurrency = new Currency(Properties.Settings.Default.CzkCode);
                            var targetCurrency = new Currency(targetCurrencyCode);
                            var value = exchangeRate / amount;

                            yield return new ExchangeRate(sourceCurrency, targetCurrency, value);
                        }

                    }
                }
            }

            yield break;
        }
    }
}
