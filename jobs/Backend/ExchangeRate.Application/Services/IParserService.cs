using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services
{
    public interface IParserService
    {
        public List<ExchangeRateBankDTO> ExchangeRateParseText(string data);
        public List<ExchangeRateBankDTO> ExchangeRateParseXml(string data);
        public List<CurrencyDTO> CurrencyParse(List<ExchangeRateBankDTO> exchangeRates);
    }
}