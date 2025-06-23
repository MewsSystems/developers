using ExchangeRateUpdater.Domain.Shared;
using ExchangeRateUpdater.Infrastructure.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ExchangeRateUpdater.Test
{
    public class IntegrationTest
    {
        [TestFixture]
        public class ExchangeRateControllerTests
        {
            private HttpClient _client;
            private Settings _settings;
            private VerifySettings _verifySettings;

            private static Settings InitializeExchangeRateProvider()
            {
                var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

                var services = new ServiceCollection();
                services.Configure<Settings>(x =>
                {
                    x.DefaultCurrency = configuration["DefaultCurrency"];
                    x.CnbUrl = configuration["cnbUrl"];
                });

                var serviceProvider = services.BuildServiceProvider();

                return serviceProvider.GetRequiredService<IOptions<Settings>>().Value;
            }

            [TearDown]
            public void TearDown()
            {
                _client?.Dispose();
            }

            [SetUp]
            public void Setup()
            {
                _settings = InitializeExchangeRateProvider();
                _client = new HttpClient();
                _verifySettings = new VerifySettings();
                _verifySettings.UseDirectory("Snapshots");
                _verifySettings.ScrubInlineDateTimes("R");
                _verifySettings.IgnoreMember("Headers");
                _verifySettings.IgnoreMember("messageId");
                _verifySettings.IgnoreMember("happenedAt");
                _verifySettings.OnFirstVerify((filePair, receivedText, approvedText) =>
                {
                    _verifySettings.AutoVerify();
                    return Task.CompletedTask;
                });
            }

            [Test]
            public async Task GetDaily_ShouldReturnOk_WithValidDate()
            {
                // Arrange
                var date = "2019-05-17";
                var url = $"{_settings.CnbUrl}/{ExchangeRateRoutes.Daily}?date={date}";

                // Act
                var response = await _client.GetAsync(url);

                // Assert
                await Verify(response, _verifySettings);
            }

            [Test]
            public async Task GetDaily_ShouldReturnFail_WithInvalidDate()
            {
                // Arrange
                var date = "2019-02-31";
                var url = $"{_settings.CnbUrl}/{ExchangeRateRoutes.Daily}?date={date}";

                // Act
                var response = await _client.GetAsync(url);

                // Assert
                await Verify(response, _verifySettings);
            }

            [Test]
            public async Task GetDailyCurrencyMonth_ShouldOk_ForValidCurrency()
            {
                // Arrange
                var url = $"{_settings.CnbUrl}/{ExchangeRateRoutes.DailyCurrencyMonth}?currency=USD&yearMonth=2019-05";

                // Act
                var response = await _client.GetAsync(url);

                // Assert
                await Verify(response, _verifySettings);
            }

            [Test]
            public async Task GetDailyCurrencyMonth_ShouldReturnEmpty_ForInvalidCurrency()
            {
                // Arrange
                var url = $"{_settings.CnbUrl}/{ExchangeRateRoutes.DailyCurrencyMonth}?currency=INVALID&yearMonth=2019-05";

                // Act
                var response = await _client.GetAsync(url);

                // Assert
                await Verify(response, _verifySettings);
            }

            [Test]
            public async Task GetDailyYear_ShouldReturnOk_WithValidYear()
            {
                // Arrange
                var year = "2019";
                var url = $"{_settings.CnbUrl}/{ExchangeRateRoutes.DailyYear}?year={year}";

                // Act
                var response = await _client.GetAsync(url);

                // Assert
                await Verify(response, _verifySettings);
            }

            [Test]
            public async Task GetDailyYear_ShouldReturnFail_WithInvalidYear()
            {
                // Arrange
                var year = "0";
                var url = $"{_settings.CnbUrl}/{ExchangeRateRoutes.DailyYear}?year={year}";

                // Act
                var response = await _client.GetAsync(url);

                // Assert
                await Verify(response, _verifySettings);
            }
        }
    }
}