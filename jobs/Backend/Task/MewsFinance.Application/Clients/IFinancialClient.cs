using MewsFinance.Domain.Models;

namespace MewsFinance.Application.Clients
{
    public interface IFinancialClient
    {
        public Task<IEnumerable<ExchangeRate>> GetExchangeRates(DateTime date);
    }
}
