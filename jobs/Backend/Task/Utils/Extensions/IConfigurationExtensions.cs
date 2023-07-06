using Microsoft.Extensions.Configuration;

namespace Utils.Extensions
{
	public static class IConfigurationExtensions
	{
		public static T GetRequiredValue<T>(this IConfiguration configuration, string key)
		{
            var section = configuration.GetSection(key);

            if (!section.Exists())
            {
                throw new InvalidOperationException($"The `{key}` parameter is missing in the configuration. Please add it.");
            }

            return section.Get<T>();
        }
	}
}

