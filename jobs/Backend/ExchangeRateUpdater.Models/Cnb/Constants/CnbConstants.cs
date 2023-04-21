namespace ExchangeRateUpdater.BusinessLogic.Models.Cnb.Constants
{
    public class CnbConstants
    {
        public const string DefaultCurrencyCode = "CZK";
        public const string SettingsSectionKey = "CnbSettings";
        public const string SettingsFieldsSeparatorKey = "FieldsSeparator";
        public const string SettingsDefaultCurrencyKey = "DefaultCurrency";
        public const string SettingsDailyExChangeRateUrlKey = "DailyExChangeRateUrl";
        public const string SettingsMonthlyFxExChangeRateUrlKey = "MonthlyFxExChangeRateUrl";
        public const string SettingsExchangeRateUnitsIndexKey = "ExchangeRateUnitsIndex";
        public const string SettingsExchangeRateCodeIndexKey = "ExchangeRateCodeIndex";
        public const string SettingsExchangeRateValueIndexKey = "ExchangeRateValueIndex";
        public const string SettingsFxExchangeRateUnitsIndexKey = "FxExchangeRateUnitsIndex";
        public const string SettingsFxExchangeRateValueIndexKey = "FxExchangeRateValueIndex";

        public static Currency GetDefaultCurrency()
        {
            return new Currency(DefaultCurrencyCode);
        }
    }
}
