using Infrastructure.Clients.CnbApi.Basetypes;

namespace Infrastructure.Clients.CnbApi
{
    public interface ICnbApiClient
    {

        Task<RatesResponse> GetExchangeRatesDaily(DateTimeOffset dateTime); 

    }
}
