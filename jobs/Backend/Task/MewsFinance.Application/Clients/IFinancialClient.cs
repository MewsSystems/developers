using MewsFinance.Domain.Models;

namespace MewsFinance.Application.Clients
{
    public interface IFinancialClient
    {
        public string TargetCurrencyCode { get; }
        public Task<Response<IEnumerable<ExchangeRate>>> GetExchangeRates(DateTime date);
    }
}
