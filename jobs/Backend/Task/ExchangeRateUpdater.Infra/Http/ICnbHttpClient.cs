using System.Threading.Tasks;
using ExchangeRateUpdater.Infra.Models;

namespace ExchangeRateUpdater.Infra.Http;

public interface ICnbHttpClient
{
    Task<Result<ExchangeRateResponse, HttpError>> GetExchangeRatesAsync();
}