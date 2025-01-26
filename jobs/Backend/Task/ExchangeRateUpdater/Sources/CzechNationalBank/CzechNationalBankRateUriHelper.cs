using System;

public static class CzechNationalBankRateUriHelper
{
    private static string DateParameterFormat = "dd.MM.yyyy";
    public static Uri BuildMainSourceUri(Uri baseUri, DateOnly targetDate)
    {
        var dateParam = $"date={targetDate.ToString(DateParameterFormat)}";
        var targetUrlBuilder = new UriBuilder(baseUri);
        targetUrlBuilder.Query = dateParam;
        return targetUrlBuilder.Uri;
    }

    public static Uri BuildSecondarySourceUri(Uri baseUri, DateOnly targetDate)
    {
        var dateParams = $"year={targetDate.Year}&month={targetDate.Month}";
        var targetUrlBuilder = new UriBuilder(baseUri);
        targetUrlBuilder.Query = dateParams;
        return targetUrlBuilder.Uri;
    }
}