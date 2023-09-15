using Infrastructure.Models.AppSettings;
using Infrastructure.Services.Concrete;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Http;

namespace Infrastructure.Services.Tests.Unit.Fixture;

internal class CzechNationalBankDataServiceFixture
{
    internal HttpClient HttpClient;
    internal Mock<ILogger<CzechNationalBankDataService>> Logger;
    internal Mock<IOptions<CzechNationalBankSettings>> Options;

    public CzechNationalBankDataServiceFixture()
    {
        HttpClient = new Mock<IHttpClientFactory>().Object.CreateClient();
        Logger = new Mock<ILogger<CzechNationalBankDataService>>();
        Options = new Mock<IOptions<CzechNationalBankSettings>>();
    }

    internal CzechNationalBankDataService CreateInstance()
    {
        return new CzechNationalBankDataService(HttpClient, Logger.Object, Options.Object);
    }

    internal CzechNationalBankDataService CreateInstance(HttpClient httpClient)
    {
        return new CzechNationalBankDataService(httpClient, Logger.Object, Options.Object);
    }
}
