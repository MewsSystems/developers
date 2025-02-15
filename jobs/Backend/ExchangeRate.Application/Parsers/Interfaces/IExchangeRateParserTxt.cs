using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Parsers.Interfaces
{
    public interface IExchangeRateParserTxt
    {
        List<ExchangeRateBankDTO> Parse(string file);
    }
}
