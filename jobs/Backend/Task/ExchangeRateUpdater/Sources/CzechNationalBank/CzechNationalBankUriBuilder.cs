using System;

public static class CzechNationalBankRateUriBuilder
{
    private static string DateParameterFormat = "dd.MM.yyyy";
    public static Uri BuildUri(Uri baseUri, DateOnly targetDate)
    {
        var dateParam = $"date={targetDate.ToString(DateParameterFormat)}";
        var targetUrlBuilder = new UriBuilder(baseUri);
        targetUrlBuilder.Query = dateParam;
        return targetUrlBuilder.Uri;
    }
}