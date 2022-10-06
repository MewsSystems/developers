namespace Common.Configuration
{
    public interface IConfigurationWrapper
    {
        string GetConfigValueAsString(string configKey, bool ignoreExceptions = false);
        T GetConfigValue<T>(string configKey, T defaultValue, bool ignoreExceptions = true);
        IEnumerable<string> GetConfigValueAsList(string configKey, string defaultValue, char delimiter = ',', bool ignoreExceptions = false);
    }
}
