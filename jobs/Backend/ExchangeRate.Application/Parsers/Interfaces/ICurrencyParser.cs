using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Parsers.Interfaces
{
    public interface ICurrencyParser
    {
        List<CurrencyDTO> Parse(List<ExchangeRateBankDTO> exchangeRates);
    }
}