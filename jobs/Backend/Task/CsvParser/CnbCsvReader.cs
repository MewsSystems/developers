using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper.Configuration;
using ExchangeRateUpdater.Extensions;
using ExchangeRateUpdater.Models;

namespace ExchangeRateUpdater.CsvParser;

public class CnbCsvReader : ICnbCsvReader
{
    /// <summary>
    /// Class is used only for csv mapping to the corresponding model
    /// </summary>
    private class CnbExchangeRateModelMap : ClassMap<CnbExchangeRateModel>
    {
        public CnbExchangeRateModelMap()
        {
            Map(m => m.Country).Name("Country");
            Map(m => m.Currency).Name("Currency");
            Map(m => m.Amount).Name("Amount");
            Map(m => m.Code).Name("Code").Convert(args => new Currency(args.Row["Code"]));
            Map(m => m.Rate).Name("Rate");
        }
    }

    private readonly ICsvFactory _csvFactory;

    public CnbCsvReader(ICsvFactory csvFactory)
    {
        _csvFactory = csvFactory.NotNull();
    }

    public IEnumerable<CnbExchangeRateModel> GetRecords(Stream stream)
    {
        using var streamReader = new StreamReader(stream);
        // first line contains information which day the exchange rates belongs to (this information could be propagated to caller)
        streamReader.ReadLine();

        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture) { Delimiter = "|" };

        using var csvReader = _csvFactory.CreateReader(streamReader, configuration);
        csvReader.Context.RegisterClassMap<CnbExchangeRateModelMap>();
            
        return csvReader.GetRecords<CnbExchangeRateModel>().ToList();
    }
}