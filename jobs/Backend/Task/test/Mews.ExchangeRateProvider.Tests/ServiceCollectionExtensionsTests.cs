using Mews.ExchangeRateProvider.Extensions;
using Mews.ExchangeRateProvider.Mappers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Mews.ExchangeRateProvider.Tests;

[TestFixture]
public sealed class ServiceCollectionExtensionsTests
{
    [Test]
    public void AddExchangeRateProvider_throws_exception_if_configuration_is_null() =>
        Assert.Throws<ArgumentNullException>(() => new ServiceCollection().AddExchangeRateProvider(null!));

    [Test]
    public void AddExchangeRateProvider_builds_container_if_configuration_is_set() =>
        Assert.DoesNotThrow(() => BuildServiceCollection().BuildServiceProvider());

    [Test]
    public void AddExchangeRateProvider_registers_mapper() =>
        Assert.DoesNotThrow(() =>
            BuildServiceCollection()
                .BuildServiceProvider()
                .GetRequiredService<CzechNationalBankExchangeRateMapper>());

    [Test]
    public void AddExchangeRateProvider_registers_client() =>
        Assert.DoesNotThrow(() =>
            BuildServiceCollection()
                .BuildServiceProvider()
                .GetRequiredService<IExchangeRateProvider>());

    [Test]
    public void AddExchangeRateProvider_reads_configuration()
    {
        var sp = BuildServiceCollection().BuildServiceProvider();

        var options = sp.GetRequiredService<IOptions<CzechNationalBankExchangeRateProviderOptions>>();

        Assert.That(options.Value.ExchangeRateProviders?.First().Uri?.AbsoluteUri, Is.EqualTo("https://test.com/a.txt"));
        Assert.That(options.Value.ExchangeRateProviders?.Last().Uri?.AbsoluteUri, Is.EqualTo("https://test.com/b.txt"));
    }

    private static IServiceCollection BuildServiceCollection()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "TestData"))
            .AddJsonFile("exampleSettings.json")
            .Build();

        var sc = new ServiceCollection();
        return sc.AddExchangeRateProvider(configuration.GetRequiredSection(CzechNationalBankExchangeRateProviderOptions.Section));
    }
}