using ExchangeRate.Application.DTOs;

namespace ExchangeRate.Application.Services
{
    public interface ICurrencyParser
    {
        List<CurrencyDTO> Parse(List<ExchangeRateBankDTO> exchangeRates);
    }
}