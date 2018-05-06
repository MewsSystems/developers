using Newtonsoft.Json.Linq;

namespace ExchangeRateUpdater
{
    public interface IExchangeRateParser
    {
        decimal? ParseExchangeRateResponse(JObject response, string rootObjectKey);
    }
}