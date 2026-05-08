using System.IO;
using System.Text.Json;

namespace ExchangeRateUpdater.Helpers
{
    internal static class ConfigHelper
    {
        public static string GetCnbApiPath()
        {
            var json = File.ReadAllText("appsettings.json");
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement
                      .GetProperty("CNB")
                      .GetProperty("ApiPath")
                      .GetString()!;
        }
    }
}