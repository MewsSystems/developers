using MewsFinance.Application.UseCases.ExchangeRates.Queries;

namespace MewsFinance.Application.Interfaces
{
    public interface IGetExchangeRatesUseCase
    {
        IEnumerable<ExchangeRateResponse> GetExchangeRates(ExchangeRateRequest exchangeRateRequest);
    }
}
