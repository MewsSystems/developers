using System.Threading.Tasks;

namespace ExchangeRateUpdater.Cnb;

public interface ICnbClient
{
    Task<CnbExchangeRatesDto> GetCurrentExchangeRates();
}