using System.Threading.Tasks;
using ExchangeRateUpdater.Decorator;
using ExchangeRateUpdater.Singleton;
using ExchangeRateUpdater.Models;
using Xunit;
using System.Runtime.CompilerServices;

namespace TestExchangeRateUpdater.Decorator
{
    public class LoadDataTest
    {
        [Fact]
        public async Task Load_AddsRatesToDB()
        {
            // Arrange
            string input = "United States|Dollar|1|USD|25.50\n" +
                           "Germany|Euro|1|EUR|27.00\n" +
                           "United Kingdom|Pound|1000|GBP|30.00";

            // Arrange
            var db = DB.GetInstance();
            // Clear DB for test isolation
            var ratesField = typeof(DB).GetField("_rates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            ratesField.SetValue(db, new Dictionary<string, Rate>());

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
            Assert.Equal(1, rate2.Amount);
            Assert.Equal("EUR", rate2.Code);
            Assert.Equal(27.00m, rate2.rate);

            Assert.True(db.TryGetValue("GBP", out Rate rate3));
            Assert.Equal("United Kingdom", rate3.Country);
            Assert.Equal("Pound", rate3.Currency);
            Assert.Equal(1000, rate3.Amount);
            Assert.Equal("GBP", rate3.Code);
            Assert.Equal(30.00m, rate3.rate);
        }

        [Fact]
        public async Task Load_EmptyData()
        {
            // Assert
            string input = string.Empty;

            DB db = DB.GetInstance();

            LoadData loader = new();

            // Act
            bool result = await loader.Load(input);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Load_Throw_ArgumentNullException()
        {
            // Assert
            LoadData load = new();

            //Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => load.Load(null));
        }

        [Theory]
        [InlineData("fail1")]
        [InlineData("fail2|fail2")]
        [InlineData("fail3|fail3|3")]
        [InlineData("fail4|fail4|4|fail4")]
        public async Task Load_Throw_IndexOutOfRangeException_String_With_Wrong_Data_Less_Than_needed(string data)
        {
            // Assert
            LoadData load = new();

            //Act & Assert
            await Assert.ThrowsAsync<IndexOutOfRangeException>(() => load.Load(data));
        }

        [Theory]
        [InlineData("fail1|fail1|fail1|fail1")]
        [InlineData("fail2|fail2|fail2|fail2|2")]
        [InlineData("fail3|fail3|3|fail3|fail3")]
        [InlineData("fail4|fail4|fail4|fail4|fail4")]
        public async Task Load_Throw_FormatException_String_With_Wrong_Data(string data)
        {
            // Assert
            LoadData load = new();

            //Act & Assert
            await Assert.ThrowsAsync<FormatException>(() => load.Load(data));
        }

    }
}