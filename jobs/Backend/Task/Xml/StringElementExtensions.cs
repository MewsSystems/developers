using System;
using System.Threading.Tasks;
using System.Xml;
using ExchangeRateUpdater.Fluent;
using ExchangeRateUpdater.Html;

namespace ExchangeRateUpdater.Xml
{
    public static class StringElementExtensions
    {
        public static async Task<Fluent<XmlElement>> ToXmlElement(
            this Task<Fluent<StringElement>> fluentElementTask)
        {
            var fluentElement = await fluentElementTask;
            
            var document = new XmlDocument();
            document.LoadXml(fluentElement.Value.Value);
            if (document.DocumentElement == null)
            {
                throw new Exception($"String element '{fluentElement.Value.Value}' " +
                                    $"could not be loaded to '{nameof(XmlDocument)}'");
            }
            return fluentElement.Create(document.DocumentElement);
        }
    }
}