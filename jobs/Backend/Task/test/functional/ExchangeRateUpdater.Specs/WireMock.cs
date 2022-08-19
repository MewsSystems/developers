using System.Linq;
using WireMock.Server;
using WireMock.Settings;

namespace ExchangeRateUpdater.Specs
{
    public static class WireMock
    {
        public static WireMockServer Server { get; private set; }
        public static string BaseUrl => Server.Urls.First();

        public static void Reset()
        {
            Server?.Stop();
            Server = WireMockServer.Start();
        }
    }
}