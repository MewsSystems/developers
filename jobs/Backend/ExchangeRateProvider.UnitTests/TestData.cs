namespace ExchangeRateProvider.UnitTests;

public class TestData
{
    private readonly static Currency BASECURRENCY = new Currency( "CZK" );
    private readonly static string currentDirectory = Directory.GetCurrentDirectory();

    public static IEnumerable<object[]> ReturnsCorrectExchangeRates
    {
        get
        {
            yield return new object[] { new Currency[] {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
            },
        new CNBApiExchangeRateRecord[]
        {
            new CNBApiExchangeRateRecord() { Currency = "dollar", Amount = 1, CurrencyCode = "USD", Country = "USA", Order = 94, Rate = 23.048M, ValidFor = DateTime.Now },
            new CNBApiExchangeRateRecord() { Currency = "Euro", Amount = 1, CurrencyCode = "EUR", Country = "EMU", Order = 94, Rate = 25.75M, ValidFor = DateTime.Now },
            new CNBApiExchangeRateRecord() { Currency = "yen", Amount = 100, CurrencyCode = "JPY", Country = "Japan", Order = 94, Rate = 21.045M, ValidFor = DateTime.Now },
            new CNBApiExchangeRateRecord() { Currency = "baht", Amount = 100, CurrencyCode = "THB", Country = "Thailand", Order = 94, Rate = 72.534M, ValidFor = DateTime.Now },
            new CNBApiExchangeRateRecord() { Currency = "lira", Amount = 1, CurrencyCode = "TRY", Country = "Turkey", Order = 94, Rate = 3.806M, ValidFor = DateTime.Now },
        } };
        }
    }

    public static IEnumerable<object[]> ReturnsDailyExchangeRates
    {
        get
        {
            yield return new object[] { new Currency[] {
            new Currency("USD"),
            new Currency("EUR"),
            new Currency("CZK"),
            new Currency("JPY"),
            new Currency("KES"),
            new Currency("RUB"),
            new Currency("THB"),
            new Currency("TRY"),
            new Currency("XYZ")
            },
            JsonConvert.DeserializeObject<CNBApiDailyExchangeRateResponse>( File.ReadAllText( "FullResponse.json" ) )?.Rates
         };
        }
    }

    public static IEnumerable<object[]> ApiRequestsAndResponses
    {
        get
        {
            return new[]
            {
                new object[]
                {
                    File.ReadAllText("FullResponse.json"),
                    HttpStatusCode.OK,
                    true,
                    31
                },
                new object[]
                {
                    File.ReadAllText("OnlyUSD.json"),
                    HttpStatusCode.OK,
                    true,
                    1
                },
                new object[]
                {
                    string.Empty,
                    HttpStatusCode.BadRequest,
                    false,
                    null
                }
            };
        }
    }

    public static IEnumerable<object[]> ApiRequestsAndResponsesRetryPolicy
    {
        get
        {
            return new[]
            {
                new object[]
                {
                    HttpStatusCode.BadRequest,
                    false,
                    null
                }
            };
        }
    }
}