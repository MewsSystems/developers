using System;
using System.Configuration;
using ExchangeRateUpdater.Parsing;

namespace ExchangeRateUpdater
{
    internal class ConfigurationProvider
    {
        public ConfigurationProvider()
        {
            if (ConfigurationManager.AppSettings != null)
            {
                var baseUrl = ConfigurationManager.AppSettings["dataSourceUrl"];

                if (!string.IsNullOrEmpty(baseUrl))
                {
                    BaseUrl = baseUrl;
                }

                var format = ConfigurationManager.AppSettings["dataSourceDateFormat"];

                if (!string.IsNullOrEmpty(format))
                {
                    DateFormat = format;
                }

                var parser = ConfigurationManager.AppSettings["dataSourceParser"];
                if (!string.IsNullOrEmpty(parser) && Enum.TryParse(parser, out ExchangeRateParserType parserType))
                {
                    Parser = parserType;
                }
            }
        }

        public string BaseUrl { get; private set; } = "https://www.cnb.cz/cs/financni-trhy/devizovy-trh/kurzy-devizoveho-trhu/kurzy-devizoveho-trhu/denni_kurz.txt?date={0}";
        public string DateFormat { get; private set; } = "dd.MM.yyyy";
        public ExchangeRateParserType Parser { get; private set; } = ExchangeRateParserType.CNB;
    }
}