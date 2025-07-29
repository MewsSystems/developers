using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Models;
using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Parsers;

public class CnbXmlParser : IExchangeRateParser
{
    public IEnumerable<ExchangeRate> Parse(string data, Currency baseCurrency)
    {
        try
        {
            var dataDoc = XDocument.Parse(data);
            var rates = dataDoc.Descendants("radek")
                .Select(currency => new ExchangeRate(
                    new Currency(currency.Attribute("kod")?.Value),
                    baseCurrency,
                    // make sure the comma is used as a decimal delimiter
                    decimal.Parse(currency.Attribute("kurz")?.Value!, CultureInfo.GetCultureInfo("cs-CZ")) /
                    int.Parse(currency.Attribute("mnozstvi")?.Value!)
                ));
            return rates;

        }
        catch (Exception ex)
        {
            throw new ParsingException("Failed to parse CNB exchange rates", ex);
        }
    }   
}