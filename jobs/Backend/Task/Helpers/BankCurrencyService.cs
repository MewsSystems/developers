using ExchangeRateUpdater.Helpers.Interfaces;
using static ExchangeRateUpdater.Enums.Banks;

namespace ExchangeRateUpdater.Helpers
{
    public class BankCurrencyService : IBankCurrencyService
    {
        public Currency FindBankCurrency(BankType bank)
        {
            switch (bank)
            {
                case BankType.CNB:
                    return new Currency("CZK");
                default:
                    return null;
            }
        }
    }
}