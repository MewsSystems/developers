using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using ExchangeRateUpdater.Providers.Cnb;
using ExchangeRateUpdater.Tests.Common;
using NSubstitute;
using Xunit;

namespace ExchangeRateUpdater.Tests.Providers;

public class CnbExchangeRateProviderTests : TestBase
{
    [Fact]
    public async Task Should_Get_ExchangeRates()
    {
        var httpClientFactoryMock = CreateHttpClientFactoryMock(() =>
        {
            string cnbRatesResponseFile = File.ReadAllText(@".\Resources\Cnb_Rates.csv");
            var fakeHttpMessageHandler = new FakeHttpMessageHandler(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(cnbRatesResponseFile, Encoding.UTF8, MediaTypeNames.Text.Plain)
            });
            return fakeHttpMessageHandler;
        });

        var cnbRateProvider = new CnbExchangeRateProvider(httpClientFactoryMock);
        var cnbRates = await cnbRateProvider.GetExchangeRatesAsync(new Domain.Currency[]
        {
            new("USD")
        });

        Assert.NotEmpty(cnbRates);
        Assert.Contains(cnbRates, x => x.SourceCurrency.Equals(new Domain.Currency("USD")));
    }
}