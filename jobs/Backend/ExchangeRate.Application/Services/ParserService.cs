using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Parsers.Interfaces;
using ExchangeRate.Application.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Services
{
    public class ParserService : IParserService
    {
        private readonly IExchangeRateParserTxt _txtParser;
        private readonly IExchangeRateParserXml _xmlParser;
        private readonly ICurrencyParser _currencyParser;

        public ParserService(IExchangeRateParserTxt txtParser, 
            IExchangeRateParserXml xmlParser,
            ICurrencyParser currencyParser )
        {
            _txtParser = txtParser;
            _xmlParser = xmlParser;
            _currencyParser = currencyParser;
        }

        public List<ExchangeRateBankDTO> ExchangeRateParseText(string data)
        {
            return _txtParser.Parse(data);
        }

        public List<ExchangeRateBankDTO> ExchangeRateParseXml(string data)
        {
            return _xmlParser.Parse(data);
        }
        public List<CurrencyDTO> CurrencyParse(List<ExchangeRateBankDTO> exchangeRates)
        {
            return _currencyParser.Parse(exchangeRates);
        }
    }
}
