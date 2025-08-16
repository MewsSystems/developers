using ExchangeRateUpdater.Domain.Enums;
using ExchangeRateUpdater.Domain.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Application.Banks
{
    public interface IBankConnector
    {
        BankIdentifier BankIdentifier { get; }
        Task<IEnumerable<ExchangeRate>> GetExchangeRates(IEnumerable<Currency> currencies);
    }
}
