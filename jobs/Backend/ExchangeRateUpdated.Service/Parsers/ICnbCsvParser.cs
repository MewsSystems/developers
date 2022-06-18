using FluentResults;

namespace ExchangeRateUpdated.Service.Parsers
{
    public interface ICnbCsvParser
    {
        Result<IEnumerable<CnbExchangeRateRecord>> TryParseExchangeRates(Stream stream);
    }
}