using ExchangeRateUpdater.api.Controllers;
using ExchangeRateUpdater.Infrastructure.Services;
using ExchangeRateUpdater.Domain.Shared;
using ExchangeRateUpdater.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using LanguageExt.Pipes;

namespace ExchangeRateUpdater.Test
{
    [TestFixture]
    public class ControllerTest
    {
        private ExchangeRateController _controller;
        private VerifySettings _verifySettings;
        private readonly char commaSeparator = ',';

        private static IExchangeRateProvider InitializeExchangeRateProvider()
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
            services.AddSingleton<ExchangeRateCacheManager>();
            services.AddSingleton<IExchangeRateProvider, ExchangeRateProvider>();
            services.AddMemoryCache();

            var serviceProvider = services.BuildServiceProvider();

            return serviceProvider.GetRequiredService<IExchangeRateProvider>();
        }
        [TearDown]
        public void TearDown()
        {
            _controller?.Dispose();
        }

        [SetUp]
        public void Setup()
        {
            _controller = new ExchangeRateController(InitializeExchangeRateProvider());
            _verifySettings = new VerifySettings();
            _verifySettings.UseDirectory("Snapshots");
            _verifySettings.IgnoreMembers("Headers");
            _verifySettings.IgnoreMember("messageId");
            _verifySettings.IgnoreMember("happenedAt");
                _verifySettings.AutoVerify();
            _verifySettings.OnFirstVerify((filePair, receivedText, approvedText) =>
            {
                return Task.CompletedTask;
            });
        }

        [Test]
        public async Task GetDaily_ShouldReturnOk_WithValidDate()
        {
            // Arrange
            var date = "2019-05-17";

            // Act
            var response = _controller.GetDaily(date) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDaily_ShouldReturnOk_WithValidCurrencies()
        {
            // Arrange
            var date = "2019-05-17";
            string currencies = "USD,EUR,CZK,JPY,KES,RUB,THB,TRY,XYZ";

            // Act
            var response = _controller.GetDaily(date, string.Join(commaSeparator, currencies)) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDaily_ShouldReturnFail_WithInvalidDate()
        {
            // Arrange
            var date = "2019-05-32";

            // Act
            var response = _controller.GetDaily(date) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDaily_ShouldReturnFail_WithInvalidCurrencies()
        {
            // Arrange
            var date = "2019-05-17";
            string currencies = "USD,EUR,CZK,JY,KES,RUB,THBL,TRY,XYZ";

            // Act
            var response = _controller.GetDaily(date, currencies) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDaily_ShouldReturnFail_WithInvalidSeparator()
        {
            // Arrange
            var date = "2019-05-17";
            string currencies = "USD;EUR;CZK;JPY;KES;RUB;THB;TRY;XYZ";

            // Act
            var response = _controller.GetDaily(date, currencies) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDailyCurrencyMonth_ShouldReturnOk_WithValidDate()
        {
            // Arrange
            var currency = "AUD";
            var date = "2019-05";

            // Act
            var response = _controller.GetDailyCurrencyMonth(currency, date) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDailyCurrencyMonth_ShouldReturnFail_WithInvalidDate()
        {
            // Arrange
            var currency = "AUD";
            var date = "2019-05-15";

            // Act
            var response = _controller.GetDailyCurrencyMonth(currency, date) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDailyCurrencyMonth_ShouldReturnFail_WithInvalidCurrency()
        {
            // Arrange
            var currency = "AU";
            var date = "2019-05";

            // Act
            var response = _controller.GetDailyCurrencyMonth(currency, date) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDailyCurrencyMonth_ShouldReturnFail_WithEmptyCurrency()
        {
            // Arrange
            var currency = string.Empty;
            var date = "2019-05";

            // Act
            var response = _controller.GetDailyCurrencyMonth(currency, date) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDailyYear_ShouldReturnOk_WithValidDate()
        {
            // Arrange
            var date = "2019";

            // Act
            var response = _controller.GetDailyYear(date) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

        [Test]
        public async Task GetDailyYear_ShouldReturnFail_WithInvalidDate()
        {
            // Arrange
            var date = "2019-11";

            // Act
            var response = _controller.GetDailyYear(date) as ObjectResult;

            // Assert
            await Verify(response, _verifySettings);
        }

    }

}
