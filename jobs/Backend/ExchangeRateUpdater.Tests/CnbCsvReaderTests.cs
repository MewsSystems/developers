using System.Globalization;
using System.Text;
using CsvHelper.Configuration;
using ExchangeRateUpdater.CsvParser;
using ExchangeRateUpdater.Models;
using NUnit.Framework;

namespace ExchangeRateUpdater.Tests;

[TestFixture]
public class CnbCsvReaderTests
{
#pragma warning disable CS8618
    private CnbCsvReader _csvReader;
    private CsvConfiguration _configuration;
#pragma warning restore CS8618
    [SetUp]
    public void Setup()
    {
        _csvReader = new CnbCsvReader(new CsvFactory());
        _configuration = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = "|" };
    }

    [Test]
    public void CnbScvReader_passValidData_validOutput()
    {
        var csv = $@"26 Nov 2021 #228
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|16.250
India|rupee|100|INR|30.362";
        var csvReader = new CnbCsvReader(new CsvFactory());
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(csv));

        var records = csvReader.GetRecords(stream).ToList();

        var e1 = new CnbExchangeRateModel
        {
            Amount = 1,
            Country = "Australia",
            Code = new Currency("AUD"),
            Currency = "dollar",
            Rate = 16.25M
        };
        var e2 = new CnbExchangeRateModel
        {
            Amount = 100,
            Country = "India",
            Code = new Currency("INR"),
            Currency = "rupee",
            Rate = 30.362M
        };

        Assert.AreEqual(2, records.Count, "Failed to parse input data");

        Assert.AreEqual(e1, records[0]);
        Assert.AreEqual(e2, records[1]);
    }
}