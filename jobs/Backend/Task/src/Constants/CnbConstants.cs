namespace ExchangeRateUpdater;

public static class CnbConstants
{
    public const string BaseCurrencyCode = "CZK";
    public const string DateFormat = "dd.MM.yyyy";

    // XML element and attribute names from CNB response (in Czech)
    public const string XmlRootElementName = "kurzy";
    public const string XmlRowElementName = "radek";
    public const string DateAttributeName = "datum";
    public const string CodeAttributeName = "kod";
    public const string AmountAttributeName = "mnozstvi";
    public const string RateAttributeName = "kurz";

    // Culture and query parameters
    public const string CzechCultureCode = "cs-CZ";
    public const string DateQueryParameter = "date";
}