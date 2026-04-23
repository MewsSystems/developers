using NLog;
using Provider.ProviderAttribute;
using System.Reflection;

namespace API.Factory
{
    public static class DynamicDependencyLoader
    {
        public static Dictionary<string, Type> LoadDependencies()
        {
            var logger = LogManager.GetCurrentClassLogger();
            var providerTypeMap = new Dictionary<string, Type>();

            string exePath = Assembly.GetExecutingAssembly().Location;
            string? directoryPath = Path.GetDirectoryName(exePath);
            if (directoryPath is not null)
            {
                foreach (string dllPath in Directory.GetFiles(directoryPath, "*.dll"))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(dllPath);
                        foreach (Type type in assembly.GetTypes())
                        {
                            var providerAttribute = type.GetCustomAttribute<ProviderTypeAttribute>();
                            if (providerAttribute is not null)
                            {
                                if(providerTypeMap.TryAdd(providerAttribute.ProviderType, type))
                                {
                                    logger.Info("'{dll}' .dll loaded in as {ProviderType}", dllPath, providerAttribute.ProviderType);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                      
                        logger.Error(ex, "Error loading assembly '{dllPath}': {ex.Message}", dllPath, ex.Message);
                        throw;
                    }
                }
            }
            return providerTypeMap;
        }
    }
}
