using System.Web;

namespace ExchangeRate.IntegrationTests.Helpers;

public static class QueryBuilder
{
    public static string BuildUriQuery(string baseUrl, params (string Key, string Value)[] @params)
    {
        var parameters = HttpUtility.ParseQueryString(string.Empty);
        foreach (var (key, value) in @params) parameters.Add(key, value);

        var query = new UriBuilder(baseUrl)
            {
                Query = parameters.ToString() ?? string.Empty
            }
            .Query;

        return Path.Combine(baseUrl, query);
    }
}