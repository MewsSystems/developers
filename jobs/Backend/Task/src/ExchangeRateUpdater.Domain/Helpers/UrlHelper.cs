namespace ExchangeRateUpdater.Domain.Helpers
{
    public static class UrlHelper
    {
        public static string GetUriFromModelWithParams<T>(string baseUrl, T model) =>
            $"{baseUrl}?{string.Join("&", model.GetType().GetProperties().Select(x => $"{x.Name}={Uri.EscapeDataString(x.GetValue(model)?.ToString())}"))}";
    }
}
