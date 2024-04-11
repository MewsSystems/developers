using MewsFinance.Application.UseCases.ExchangeRates.Queries;

namespace MewsFinance.Application.Interfaces
{
    public interface IGetExchangeRatesUseCase
    {
        Task<IEnumerable<ExchangeRateResponse>> GetExchangeRates(ExchangeRateRequest exchangeRateRequest);
    }
}
