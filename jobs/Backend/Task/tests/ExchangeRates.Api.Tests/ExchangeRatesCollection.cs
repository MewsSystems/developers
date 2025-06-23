namespace ExchangeRates.Api.Tests;

[CollectionDefinition(nameof(ExchangeRatesTestsContext))]
public class DatabaseCollection : ICollectionFixture<ExchangeRatesTestsContext>
{
}