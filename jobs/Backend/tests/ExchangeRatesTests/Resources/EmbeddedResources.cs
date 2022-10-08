using System.Reflection;
using System.Text;

namespace ExchangeRates.Tests.Resources
{
	internal static class EmbeddedResource
	{
		internal static string GetResource(string pathAndFileName)
		{
			try
			{
				var assembly = typeof(EmbeddedResource).GetTypeInfo().Assembly;
				var fullName = $"ExchangeRates.Tests.Resources.{pathAndFileName}";
				using (var stream = assembly.GetManifestResourceStream(fullName))
				{
					if (stream == null) 
					{
						throw new Exception("Could not read the desirable resources.");
					}
					using (var reader = new StreamReader(stream, Encoding.UTF8))
					{
						return reader.ReadToEnd();
					}				
				}				
			}
			catch (Exception exception)
			{
				throw new Exception($"Failed to read Embedded Resource {pathAndFileName}", exception);
			}
		}
	}
}
