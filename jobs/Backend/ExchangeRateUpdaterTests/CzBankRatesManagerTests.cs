using ExchangeRateUpdater.BankRatesManagers;
using ExchangeRateUpdater.Models;
using Newtonsoft.Json;

namespace Tests;

[TestClass]
public class CnbRatesManagerTests
{
    private IBankRatesManager ratesManager;

    [TestInitialize]
    public void Setup()
    {
        ratesManager = new CnbRatesManager();
    }

    [TestMethod]
    public void ParseLine_CorrectLine_ParsedCorrectly()
    {
        // arrange
        var expectedExchangeRate = new ExchangeRate(new Currency("CZ"), new Currency("AUD"), 15.546m);
        var lineToParse = $"Australia|dollar|1|{expectedExchangeRate.TargetCurrency}|{expectedExchangeRate.Value}";

        // act
        var result = ratesManager.ParseLine(lineToParse);

        // verify
        Assert.AreEqual(expectedExchangeRate, result);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ParseLine_WrongColumnCount_ExceptionThrown()
    {
        // arrange
        var lineToParse = "dollar|1|AUD|15.546";

        // act and verify by method attribute
        ratesManager.ParseLine(lineToParse);
    }

    [DataTestMethod]
    [DataRow("Data/example1.txt", "Data/example1_result.txt")]
    [DataRow("Data/example2.txt", "Data/example2_result.txt")]
    public async Task Parse(string inputPath, string expectedOutputPath)
    {
        // arrange
        var inputString = await File.ReadAllTextAsync(inputPath);
        var expectedOutputString = await File.ReadAllTextAsync(expectedOutputPath);
        var expectedOutputObject = JsonConvert.DeserializeObject<List<ExchangeRate>>(expectedOutputString);

        // act
        var result = ratesManager.Parse(inputString ?? string.Empty);

        // assert
        CollectionAssert.AreEquivalent(expectedOutputObject, result.ToList());
    }
}