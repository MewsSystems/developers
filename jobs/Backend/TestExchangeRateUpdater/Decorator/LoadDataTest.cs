using System.Threading.Tasks;
using ExchangeRateUpdater.Decorator;
using ExchangeRateUpdater.Singleton;
using ExchangeRateUpdater.Models;
using Xunit;

namespace TestExchangeRateUpdater.Decorator
{
    public class LoadDataTest
    {
        [Fact]
        public async Task Load_AddsRatesToDB()
        {
            // Arrange
            string input = "United States|Dollar|1|USD|25.50\n" +
                           "Germany|Euro|100|EUR|27.30\n" +
                           "United Kingdom|Pound|1000|GBP|30.00";

            DB db = DB.GetInstance();

            LoadData loader = new();

            // Act
            bool result = await loader.Load(input);

            // Assert
            Assert.True(result);

            Assert.True(db.TryGetValue("USD", out Rate rate1));
            Assert.Equal("United States", rate1.Country);
            Assert.Equal("Dollar", rate1.Currency);
            Assert.Equal(1, rate1.Amount);
            Assert.Equal("USD", rate1.Code);
            Assert.Equal(25.50m, rate1.rate);

            Assert.True(db.TryGetValue("EUR", out Rate rate2));
            Assert.Equal("Germany", rate2.Country);
            Assert.Equal("Euro", rate2.Currency);
            Assert.Equal(100, rate2.Amount);
            Assert.Equal("EUR", rate2.Code);
            Assert.Equal(27.30m, rate2.rate);

            Assert.True(db.TryGetValue("GBP", out Rate rate3));
            Assert.Equal("United Kingdom", rate3.Country);
            Assert.Equal("Pound", rate3.Currency);
            Assert.Equal(1000, rate3.Amount);
            Assert.Equal("GBP", rate3.Code);
            Assert.Equal(30.00m, rate3.rate);
        }
    }
}