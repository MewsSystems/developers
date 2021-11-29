using System;

namespace ExchangeRateUpdater.Helpers;

public static class UrlFormatter
{
    /// <summary>
    /// Merge url with query parameters into single url.
    /// If the parameters contains a following macros they will be converted to corresponding datetime value
    /// %YEAR%"
    /// %MONTH%
    /// %DAY%
    /// </summary>
    public static string Construct(DateTime dateTime, string url, string parameters = null)
    {
        if (string.IsNullOrEmpty(parameters))
            return url;
            
        parameters = parameters.Replace("%YEAR%", dateTime.Year.ToString());
        parameters = parameters.Replace("%MONTH%", dateTime.Month.ToString());
        parameters = parameters.Replace("%DAY%", dateTime.Day.ToString());

        return $"{url}?{parameters}";
    }
}