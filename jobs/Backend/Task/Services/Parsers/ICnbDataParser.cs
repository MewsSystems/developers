using ExchangeRateUpdater.Models;
using FluentResults;

namespace ExchangeRateUpdater.Services.Parsers;

public interface ICnbDataParser
{
    Result<CnbExchangeRateData> Parse(string rawData);
}
