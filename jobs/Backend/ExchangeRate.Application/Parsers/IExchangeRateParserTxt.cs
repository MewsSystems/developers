using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Parsers
{
    public interface IExchangeRateParserTxt
    {
        List<ExchangeRateBankDTO> Parse(string file);
    }
}
