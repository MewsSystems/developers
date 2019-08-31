using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace ExchangeRateUpdater
{
    public static class ExchangeRateParser
    {
        public static IEnumerable<ExchangeRate> Parse(string document)
        {
            try
            {
                var result = new List<ExchangeRate>();
                var xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(document);
                var parentNode = xmlDoc.GetElementsByTagName("radek");
                foreach (XmlNode childrenNode in parentNode)
                {
                    try
                    {
                        result.Add(new ExchangeRate(new Currency(childrenNode.Attributes["kod"].Value),
                                                    new Currency("CZK"),
                                                    decimal.Parse(childrenNode.Attributes["kurz"].Value, CultureInfo.GetCultureInfo("cs-CZ"))
                                                    / int.Parse(childrenNode.Attributes["mnozstvi"].Value)));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Could not parse exchange rate: '{e.Message}'.");
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Document parsing error: '{e.Message}'.");
                return null;
            }
        }
    }
}
