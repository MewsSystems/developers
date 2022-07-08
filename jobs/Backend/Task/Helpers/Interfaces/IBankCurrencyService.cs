using static ExchangeRateUpdater.Enums.Banks;

namespace ExchangeRateUpdater.Helpers.Interfaces
{
    public interface IBankCurrencyService
    {
        Currency FindBankCurrency(BankType bank);
    }
}