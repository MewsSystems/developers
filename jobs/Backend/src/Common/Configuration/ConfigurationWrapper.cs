using Microsoft.Extensions.Configuration;
using System.ComponentModel;

namespace Common.Configuration
{
    /// <summary>
    /// Wrapper with common configuration helpers
    /// </summary>
    public class ConfigurationWrapper : IConfigurationWrapper
    {
        private IConfiguration _configuration;
        public ConfigurationWrapper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Get config value as string
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="ignoreExceptions"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string GetConfigValueAsString(string configKey, bool ignoreExceptions = false)
        {
            string configValue = _configuration.GetValue<string>(configKey);

            if (string.IsNullOrWhiteSpace(configValue) && !ignoreExceptions)
            {
                throw new Exception(string.Format("The {0} key is not present in the config, or it does not have a value.", configKey));
            }

            return configValue;
        }


        /// <summary>
        /// Allows us to get a config, specifying a default if not set
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="ignoreExceptions"></param>
        /// <returns>Value from config, or default</returns>
        /// <exception cref="Exception"></exception>
        public T GetConfigValue<T>(string configKey, T defaultValue, bool ignoreExceptions = true)
        {
            string configValue = _configuration.GetValue<string>(configKey);

            if (string.IsNullOrWhiteSpace(configValue))
            {
                return defaultValue;
            }

            try
            {
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                return (T)converter.ConvertFrom(configValue);
            }
            catch (Exception exception)
            {
                if (!ignoreExceptions)
                {                    
                    throw new Exception(string.Format("The value [{0}] for key [{1}] could not be parsed to type [{2}].", configValue, configKey, typeof(T)));
                }

                return defaultValue;
            }
        }

        /// <summary>
        /// Helper to return char delimitted config value as list
        /// </summary>
        /// <param name="configKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="delimiter"></param>
        /// <param name="ignoreExceptions"></param>
        /// <returns>List of strings</returns>
        public IEnumerable<string> GetConfigValueAsList(string configKey, string defaultValue, char delimiter = ',', bool ignoreExceptions = false)
        {
            return GetConfigValue<string>(configKey, defaultValue, ignoreExceptions).Split(delimiter).Select(str => str.Trim()).ToList();
        }
    }
}
