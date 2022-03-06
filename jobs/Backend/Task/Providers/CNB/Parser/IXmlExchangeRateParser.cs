using System.Collections.Generic;
using System.IO;
using ExchangeRateUpdater.Domain;

namespace ExchangeRateUpdater.Providers.CNB;

public interface IXmlExchangeRateParser
{
    List<ExchangeRate> Parse(string document);
    List<ExchangeRate> Parse(Stream stream);
}