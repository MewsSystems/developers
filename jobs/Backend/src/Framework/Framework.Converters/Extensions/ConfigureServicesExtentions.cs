using Framework.Converters.Abstract;
using Microsoft.Extensions.DependencyInjection;

namespace Framework.Converters.Extensions
{
	public static class ConfigureServicesExtensions
	{
		public static void AddFrameworkConverters(this IServiceCollection services)
		{
			services.AddScoped<IXmlConverter, XmlConverter>();
		}
	}
}
