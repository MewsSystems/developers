using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Parsers.Interfaces;
using System.Globalization;
using System.Xml.Linq;

namespace ExchangeRate.Application.Parsers
{
    public class ExchangeRateParserXml : IExchangeRateParserXml
    {
        public List<ExchangeRateBankDTO> Parse(string xmlData)
        {
            var xmlDoc = XDocument.Parse(xmlData);

            return xmlDoc.Descendants("radek")
                .Select(x => new ExchangeRateBankDTO
                {
                    Country = (string)x.Attribute("zeme")!,
                    Currency = (string)x.Attribute("mena")!,
                    Amount = int.Parse((string)x.Attribute("mnozstvi")!),
                    Code = (string)x.Attribute("kod")!,
                    Rate = decimal.Parse(((string)x.Attribute("kurz")!).Replace(',', '.'), CultureInfo.InvariantCulture)
                })
                .ToList();
        }
    }
}
