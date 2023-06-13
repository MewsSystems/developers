using System.Reflection;
using System.Text;

namespace ExchangeRateUpdater.Tests.Resources;

internal class EmbeddedResource
{
	public static string GetResource(string pathAndFileName)
	{
		try
		{
			var embeddedResourceType = typeof(EmbeddedResource);
			var assembly = embeddedResourceType.Assembly;
			var fullName = $"{embeddedResourceType.Namespace}.{pathAndFileName}";
			using var stream = assembly.GetManifestResourceStream(fullName);
			using var reader = new StreamReader(stream, Encoding.UTF8);
			return reader.ReadToEnd();
		}
		catch (Exception exception)
		{
			throw new Exception($"Failed to read Embedded Resource {pathAndFileName}", exception);
		}
	}
}