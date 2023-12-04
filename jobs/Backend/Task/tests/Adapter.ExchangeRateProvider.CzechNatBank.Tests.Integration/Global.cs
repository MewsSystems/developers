using NUnit.Framework;
using Serilog;
using YamlDotNet.Serialization;

namespace Adapter.ExchangeRateProvider.CzechNatBank.Tests.Integration;

[SetUpFixture]
internal class Global
{
    internal static Settings? Settings { get; set; }

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        string yamlContent = File.ReadAllText("settings.yaml");
        var deserializer = new DeserializerBuilder().Build();
        Settings = deserializer.Deserialize<Settings>(yamlContent);
    }
}
