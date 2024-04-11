using MewsFinance.Domain.Models;

namespace MewsFinance.Application.Clients
{
    public interface IFinancialClient
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(DateTime date);
    }
}
