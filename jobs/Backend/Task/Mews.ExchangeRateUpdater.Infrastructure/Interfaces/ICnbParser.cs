using Mews.ExchangeRateUpdater.Domain.ValueObjects;
using Mews.ExchangeRateUpdater.Infrastructure.Dtos;

namespace Mews.ExchangeRateUpdater.Infrastructure.Interfaces;

public interface ICnbParser
{
    IEnumerable<ExchangeRate> Parse(CnbResponse? response);
}