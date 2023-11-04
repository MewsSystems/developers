using System.Threading;
using System.Threading.Tasks;
using W4k.Either;

namespace ExchangeRateUpdater.Cnb;

public interface ICnbClient
{
    Task<Either<CnbExchangeRatesDto, CnbError>> GetCurrentExchangeRates(CancellationToken cancellationToken);
}