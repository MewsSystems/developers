using Microsoft.Extensions.Configuration;

namespace Utils.Extensions
{
	public static class IConfigurationExtensions
	{
        /// <summary>
        /// Gets the value of the <paramref name="key"/>, if it exists.
        /// </summary>
        /// <typeparam name="T">Desired type to get the configuration value.</typeparam>
        /// <param name="configuration">Configuration source.</param>
        /// <param name="key">Configuration parameter of the key.</param>
        /// <returns>Returns the configuration parameter as <typeparamref name="T"/> if exists. Throws exception if it is missing or it is empty.</returns>
        /// <exception cref="InvalidOperationException"></exception>
		public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
		{
            var section = configuration.GetSection(key);

            if (!section.Exists())
            {
                throw new InvalidOperationException($"The `{key}` parameter is missing or is empty in the configuration. Please add it.");
            }

            return section.Get<T>();
        }
	}
}

