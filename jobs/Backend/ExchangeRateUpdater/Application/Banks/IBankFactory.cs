using ExchangeRateUpdater.Domain.Enums;

namespace ExchangeRateUpdater.Application.Banks
{
    public interface IBankFactory
    {
        IBankConnector Create(BankIdentifier bankId);
    }
}
