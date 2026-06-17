namespace Mews.ExchangeRateProvider.Api.Utils
{
    /// <summary>
    /// Check for selected language, default is Czech
    /// </summary>
    public static class LanguageHelper
    {
        public const string DefaultLang = "CZ";

        public static string ParseLang(string? lang)
        {
            if (string.IsNullOrWhiteSpace(lang))
            {
                return DefaultLang;
            }
            return lang.ToUpper().Contains("EN") ? "EN" : DefaultLang;
        }
    }
}
