using Bogus;
using ExchangeRateUpdater.Exchanges.Mappers;
using ExchangeRateUpdater.Model.Cnb;
using System.Text.Json;

namespace ExchangeRateUpdater.Tests.Exchanges.Mappers;

public class CnbResponseMapperTests
{
    [Fact]
    public void MapToExchangeRates_ValidInputs()
    {
        var faker = new Faker<CnbRate>()
           .RuleFor(r => r.ValidFor, f => f.Date.Past(5))
           .RuleFor(r => r.Order, f => f.Random.Int(1, 100)) 
           .RuleFor(r => r.Country, f => f.Address.Country())
           .RuleFor(r => r.Currency, f => f.Finance.Currency().Description) 
           .RuleFor(r => r.Amount, f => f.Random.Long(1, 10)) 
           .RuleFor(r => r.CurrencyCode, f => f.Finance.Currency().Code) 
           .RuleFor(r => r.RateVal, f => f.Finance.Amount(1, 100, 3));

        var rates = new List<CnbRate>
        {
            faker.Generate(),
            faker.Generate()
        };

        var jsonObj = new RateResponse { Rates = rates };
        var jsonString = JsonSerializer.Serialize(jsonObj);


        var mappedRates = CnbDailyRateResponseMapper.MapToExchangeRates(jsonString).ToList();


        for (int i = 0; i < rates.Count(); i++)
        {
            Assert.True(rates[i].ValidFor.Equals(mappedRates[i].ValidFor));
            Assert.Equal(rates[i].Order, mappedRates[i].Order);
            Assert.Equal(rates[i].Country, mappedRates[i].Country);
            Assert.Equal(rates[i].Currency, mappedRates[i].Currency);
            Assert.Equal(rates[i].Amount, mappedRates[i].Amount);
            Assert.Equal(rates[i].CurrencyCode, mappedRates[i].CurrencyCode);
            Assert.Equal(rates[i].RateVal, mappedRates[i].RateVal);
        }
    }

    [Fact]
    public void MapToExchangeRates_JsonBadFormat()
    {
        var badFormatJson = "{123,";

        try
        {
            CnbDailyRateResponseMapper.MapToExchangeRates(badFormatJson);
        }
        catch (JsonException)
        {
            return;
        }

        Assert.Fail("Expected result to throw a JsonException when badly formatted json");
    }

    [Fact]
    public void MapToExchangeRates_ValidJsonWithIncorrectModel()
    {
        var invalidJson =
                """
                {
                    "Foo": "",
                    "Bar": {
                    }
                }
                """;

        try
        {
            CnbDailyRateResponseMapper.MapToExchangeRates(invalidJson);
        }
        catch (InvalidOperationException ex)
        {
            Assert.Contains("Failed to deserialize CNB data.", ex.Message);
            return;
        }

        Assert.Fail("Expected result to throw exception when json not matching model: " + typeof(CnbRate));
    }
}
