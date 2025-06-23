namespace ExchangeRateUpdater.Api.Authorization
{
    public interface IApiKeyValidation
    {
        bool IsValidApiKey(string userApiKey);
    }
}
