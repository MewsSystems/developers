using ExchangeRateUpdater.Infrastucture.Data.API.Entities;

namespace ExchangeRateUpdater.Infrastucture.Data.API.Abstractions
{
    public interface IExternalAPIService
    {
        Task<RatesDTO> GetFromExternalApi();
    }
}
