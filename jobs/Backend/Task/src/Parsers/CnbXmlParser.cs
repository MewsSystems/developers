using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using ExchangeRateUpdater.Exceptions;
using ExchangeRateUpdater.Interfaces;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.Parsers;

public class CnbXmlParser : IExchangeRateParser
{
    public List<ExchangeRate> Parse(string data, Currency baseCurrency)
    {
        try
        {
            var dataDoc = XDocument.Parse(data);
            return dataDoc.Descendants("radek").Select(currency => new ExchangeRate(
                    baseCurrency,
                    new Currency(currency.Attribute("kod")!.Value),
                    // make sure the comma is used as a decimal delimiter
                    decimal.Parse(currency.Attribute("kurz")?.Value!, CultureInfo.GetCultureInfo("cs-CZ")) /
                    int.Parse(currency.Attribute("mnozstvi")!.Value)
                )).ToList();
        }
        catch (Exception ex)
        {
            throw new ParsingException("Failed to parse CNB exchange rates", ex);
        }
    }   
}