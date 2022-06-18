namespace ExchangeRateUpdated.Service.Parsers
{
    public interface ICnbCsvParser
    {
        IEnumerable<CnbExchangeRateRecord> ParseExchangeRates(Stream stream);
    }
}