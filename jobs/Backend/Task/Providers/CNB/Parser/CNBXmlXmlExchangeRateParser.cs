using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Providers.CNB.Parser.Xml.Elements;
using ExchangeRateUpdater.Util;
using Microsoft.Extensions.Logging;

namespace ExchangeRateUpdater.Providers.CNB.Parser.Xml;

public class CNBXmlXmlExchangeRateParser : IXmlExchangeRateParser
{
    private const string DefaultCurrencyCode = "CZK";

    private readonly ILogger<CNBXmlXmlExchangeRateParser> _logger;

    public CNBXmlXmlExchangeRateParser(ILogger<CNBXmlXmlExchangeRateParser> logger)
    {
        _logger = logger;
    }

    public List<ExchangeRate> Parse(string document)
    {
        var parsedDocument = ParseXml(document);
        return ConvertToExchangeRates(parsedDocument).ToList();
    }

    public List<ExchangeRate> Parse(Stream stream)
    {
        // todo
        throw new NotImplementedException();
    }

    private Rates ParseXml(string xmlText)
    {
        if (string.IsNullOrWhiteSpace(xmlText))
            throw new ArgumentException("Provided XML cannot be null or whitespace.", nameof(xmlText));
        try
        {
            var serializer = new XmlSerializer(typeof(Rates));

            using var reader = new StringReader(xmlText);
            var rates = (Rates)serializer.Deserialize(reader);
            return rates;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to parse XML {xmlText}.", ex);
            throw;
        }
    }

    private static IEnumerable<ExchangeRate> ConvertToExchangeRates(Rates rates)
    {
        return rates?.Table?.Rows.Select(r => new ExchangeRate(new Currency(r.Code), new Currency(DefaultCurrencyCode), r.Rate.ToDecimal() / r.Amount)) ?? Enumerable.Empty<ExchangeRate>();
    }
}