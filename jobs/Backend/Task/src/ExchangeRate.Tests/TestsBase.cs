using AutoFixture;

namespace ExchangeRate.Tests;

public class TestsBase
{
    public Fixture Fixture = null!;
    public Random Random = new();
    
    [SetUp]
    public void Setup()
    {
        Fixture = new Fixture();

    }

    public List<ExchangeRate.Infrastructure.Cnb.Models.ExchangeRate> GenerateExchangeRates(DateTime validFor, int count = 3)
    {
        var exchangeRateFixture = Fixture.Build<ExchangeRate.Infrastructure.Cnb.Models.ExchangeRate>()
            .With(x => x.ValidFor, validFor.ToString("yyyy-MM-dd"))
            .With(x => x.CurrencyCode, GetRandomCurrencyCode)
            .With(x => x.Rate, GetRandomExchangeRate )
            .CreateMany(count)
            .ToList();

        return exchangeRateFixture;
    }
    
    private string GetRandomCurrencyCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        var randomCode = Random.GetItems<char>(chars, 3);

        return new string(randomCode);
    }
    
    private double GetRandomExchangeRate()
    {
        var randomNumber = Random.NextDouble() * 100;
        var roundedNumber = Math.Round(randomNumber, 3);

        return roundedNumber;
    }
}