using System;
using System.IO;
using System.Xml.Serialization;
using ExchangeRateUpdater.CNB.Xml.Elements;

namespace ExchangeRateUpdater.CNB.Xml;

internal static class CNBRateXmlParser
{
    public static Rates ParseXml(string xmlText)
    {
        if (string.IsNullOrWhiteSpace(xmlText))
            throw new ArgumentException("Provided XML cannot be null or whitespace.", nameof(xmlText));
        var serializer = new XmlSerializer(typeof(Rates));

        using var reader = new StringReader(xmlText);
        var rates = (Rates)serializer.Deserialize(reader);
        return rates;
    }
}