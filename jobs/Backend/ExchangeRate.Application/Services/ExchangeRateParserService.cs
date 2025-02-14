using ExchangeRate.Application.DTOs;
using ExchangeRate.Application.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRate.Application.Services
{
    public class ExchangeRateParserService
    {
        private readonly IExchangeRateParserTxt _txtParser;
        private readonly IExchangeRateParserXml _xmlParser;

        public ExchangeRateParserService(IExchangeRateParserTxt txtParser, IExchangeRateParserXml xmlParser)
        {
            _txtParser = txtParser;
            _xmlParser = xmlParser;
        }

        public List<ExchangeRateBankDTO> ParseText(string data)
        {
            return _txtParser.Parse(data);
        }

        public List<ExchangeRateBankDTO> ParseXml(string data)
        {
            return _xmlParser.Parse(data);
        }
    }
}
