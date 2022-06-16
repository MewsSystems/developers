namespace ExchangeRateUpdater.DI
{
    using System;
    using System.Configuration;
    using System.Globalization;

    internal static class Configuration
    {
        public static Uri BankUrl => new Uri(ConfigurationManager.AppSettings.Get("bankursl") 
            ?? "http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml");

        public static NumberFormatInfo NumberSeprator => 
            new NumberFormatInfo { 
                NumberDecimalSeparator = ConfigurationManager.AppSettings.Get("numberSeparator") ?? "," 
            };
    }
}
