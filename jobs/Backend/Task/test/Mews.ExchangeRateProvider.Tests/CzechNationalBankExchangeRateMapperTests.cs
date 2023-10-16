using Mews.ExchangeRateProvider.Mappers;
using System.Reflection;

namespace Mews.ExchangeRateProvider.Tests;

[TestFixture]
public sealed class CzechNationalBankExchangeRateMapperTests
{
    private readonly CzechNationalBankExchangeRateMapper _sut = new();

    private string TestData { get; set; }

    [OneTimeSetUp]
    public void Init()
    {
        var testDataFilePath = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().Location).LocalPath), "TestData", "ExampleData.txt");
        TestData = File.ReadAllText(testDataFilePath);
    }

    [TestCase("AUD", 14.805)]
    [TestCase("INR", 0.28152)]
    [TestCase("ISK", 0.16840)]
    [TestCase("IDR", 0.001495)]
    public void MapsValuesAndCalculatesRealRatesCorrectly(string sourceCurrency, decimal rate)
    {
        var results = _sut.Read(TestData);

        Assert.That(results.Count(), Is.EqualTo(31));
        Assert.That(results.Single(er => er.SourceCurrency == new Currency(sourceCurrency)).TargetCurrency, Is.EqualTo(new Currency("CZK")));
        Assert.That(results.Single(er => er.SourceCurrency == new Currency(sourceCurrency)).Value, Is.EqualTo(rate));
    }
}