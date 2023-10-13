using ExchangeRateUpdater.Domain.Enums;

namespace ExchangeRateUpdater.Domain.Settings
{
    public class AppSettings
    {
        public string[] Currencies { get; set; }
        public BankIdentifier ActiveBank { get; set; }
        public BankConfiguration[] BankConfigurations { get; set; }
    }

    public class BankConfiguration
    {
        public BankIdentifier Id { get; set; }
        public string BankId { get; set; }
        public string ExchangeRateApiUrl { get; set; }
    }
}
