using System.Reflection;
using System.Web;
using ExchangeRate.Domain.Providers;

namespace ExchangeRate.Domain.Extensions;

public static class QueryStringExtensions
{
    public static Uri ConstructRequestUri(this IExchangeRateProviderRequest request, Uri baseAddress)
    {
        ArgumentNullException.ThrowIfNull(request);

        var query = request.ToQueryString();
        var uriBuilder = new UriBuilder(baseAddress) { Query = query };
        return uriBuilder.Uri;
    }

    private static string ToQueryString(this IExchangeRateProviderRequest request)
    {
        if (request == null)
            return string.Empty;

        var properties = request.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var parameters = HttpUtility.ParseQueryString(string.Empty);

        foreach (var property in properties)
        {
            var value = property.GetValue(request);
            if (value != null) parameters.Add(property.Name, value.ToString());
        }

        return parameters.ToString();
    }
}