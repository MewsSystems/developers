using System;
using System.IO;
using Flurl;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ExchangeRateUpdaterApi.Configuration;

public static class ExchangeRateUpdaterApiConfiguration
{
    public static Settings GetExchangeRateUpdaterApiSettings()
    {
        const string fileName = "settings.yaml";

        var path = AppDomain.CurrentDomain.BaseDirectory.AppendPathSegment(fileName);
        var file = File.ReadAllText(path);
        var stringReader = new StringReader(file);

        var deserializer = new DeserializerBuilder()
            .WithNamingConvention(new PascalCaseNamingConvention())
            .Build();

        var settings = deserializer.Deserialize<Settings>(stringReader);

        return settings;
    }
}