using Flurl;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Adapter.Http.CNB.Tests.Integration.Configuration;

public static class CNBSettingsConfiguration
{
    public static CNBSettings GetCNBSettings()
    {
        const string fileName = "settings.yaml";

        var path = AppDomain.CurrentDomain.BaseDirectory.AppendPathSegment(fileName);
        var file = File.ReadAllText(path);
        var stringReader = new StringReader(file);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(new PascalCaseNamingConvention())
            .Build();

        var settings = deserializer.Deserialize<CNBSettings>(stringReader);

        return settings;
    }
}