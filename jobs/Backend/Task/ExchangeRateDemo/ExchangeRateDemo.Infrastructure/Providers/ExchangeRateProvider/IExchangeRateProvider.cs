using ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates.Models;

namespace ExchangeRateDemo.Infrastructure.Providers.ExchangeRateProvider
{
    public interface IExchangeRateProvider : IRestProvider
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates(string date);
    }
}
