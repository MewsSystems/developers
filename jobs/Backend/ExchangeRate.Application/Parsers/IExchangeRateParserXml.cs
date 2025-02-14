using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Parsers
{

    public interface IExchangeRateParserXml
    {
        List<ExchangeRateBankDTO> Parse(string xmlData);
    }
}
