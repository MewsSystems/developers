using System.IO;
using System.Reflection;

namespace ExchangeRateUpdater.IntegrationTests;

public static class ResourceReader
{
    private static readonly Assembly CurrentAssembly = Assembly.GetExecutingAssembly();

    public static string Read(string filename)
    {
        var resourceName = CurrentAssembly.GetName().Name + "." + filename;
        return Read(CurrentAssembly, resourceName);
    }

    private static string Read(Assembly assembly, string resourceName)
    {
        var resourceString = string.Empty;
        using var stream = assembly.GetManifestResourceStream(resourceName);
        if(stream == null) return resourceString;
        using var reader = new StreamReader(stream);
        resourceString = reader.ReadToEnd();
        return resourceString;
    }
}