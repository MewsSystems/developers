using System.Collections.Generic;
using System.Threading.Tasks;
using ExchangeRate.Infrastructure.CNB.Core.Models;
using ExchangeRate.Infrastructure.CNB.Core.Repositories;
using Moq;
using Xunit;

namespace ExchangeRate.UnitTests.Infrastructure;

public class ExchangeRateRepositoryTests
{
    [Fact]
    public async Task Returns_ExchangeRatesRows_FromRepository()
    {
        var tableInMemoryDatabase = GetExchangeRateRows();
        var repository = new Mock<IExchangeRateRepository>();

        repository.Setup(r => r.GetExchangeRatesAsync()).ReturnsAsync(tableInMemoryDatabase);

        var result = await repository.Object.GetExchangeRatesAsync();

        Assert.NotNull(result);
        Assert.NotNull(result.Table);
        Assert.NotNull(result.Table.Rows);

        var firstRow = result.Table.Rows[0];

        Assert.Equal(2, result.Table.Rows.Count);
        Assert.Equal("AUD", firstRow.Code);
        Assert.Equal("dolar", firstRow.CurrencyName);
        Assert.Equal(1, firstRow.Amount);
        Assert.Equal((decimal)16.518, firstRow.Rate, 4);
        Assert.Equal("Austrálie", firstRow.Country);
    }

    private static ExchangeRate.Infrastructure.CNB.Core.Models.ExchangeRate GetExchangeRateRows()
        => new()
        {
            Table = new Table
            {
                Rows = new List<Row>
                {
                    new()
                    {
                        Code = "AUD",
                        CurrencyName = "dolar",
                        Amount = 1,
                        Rate = (decimal)16.518,
                        Country = "Austrálie"
                    },
                    new()
                    {
                        Code = "BRL",
                        CurrencyName = "real",
                        Amount = 1,
                        Rate = (decimal)4.635,
                        Country = "Brazílie"
                    }
                }
            }
        };
}
