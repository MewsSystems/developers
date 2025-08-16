using ExchangeRateUpdater.ExchangeRateAPI.DTOs;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.ExchangeRateAPI.CBNClientApi
{
    public interface ICBNClientApi
    {
        public Task<ExchangeRatesResponseDTO> GetExratesDaily();
    }
}
