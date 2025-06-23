using ExchangeRateUpdater.Domain;
using ExchangeRateUpdater.Service.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExchangeRateUpdater.IntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(ICnbService));

                if (descriptor != null)
                    services.Remove(descriptor);

                var mock = new Mock<ICnbService>();

                mock.Setup(s => s.GetExchangeRatesByCurrencyAsync(It.IsAny<DateTime>(), It.IsAny<IEnumerable<Currency>>()))
                    .ReturnsAsync((DateTime date, IEnumerable<Currency> currencies) =>
                    {
                        var czk = Currency.Create("CZK");

                        var rates = new List<ExchangeRate>();
                        foreach (var currency in currencies)
                        {
                            rates.Add(ExchangeRate.Create(czk, currency, 24.0m));
                        }
                        return rates;
                    });

                services.AddSingleton(mock.Object);
            });
        }
    }
}
