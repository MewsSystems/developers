namespace CurrencyExchangeService
{
    internal static class Constants
    {
        internal static string CNBExchangeRateApiUrl = "https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml";
        internal static string CZKCurrencyCode = "CZK";

        // could be enum if more keys
        internal static string ApiResponseCacheKey = "Curencies";
    }
}
