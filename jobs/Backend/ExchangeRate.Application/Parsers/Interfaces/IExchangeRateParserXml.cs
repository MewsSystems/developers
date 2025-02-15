using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Parsers.Interfaces
{

    public interface IExchangeRateParserXml
    {
        List<ExchangeRateBankDTO> Parse(string xmlData);
    }
}
