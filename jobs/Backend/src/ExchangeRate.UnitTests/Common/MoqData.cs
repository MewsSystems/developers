using ExchangeRate.Client.Cnb.Models;
using Framework.BaseHttpClient.Models;
using Microsoft.Extensions.Options;
using Moq;

namespace ExchangeRate.UnitTests.Common
{
	public static class MoqData
	{
		public static void SetupMockCnbConfiguration(Mock<IOptions<CnbClientConfiguration>> mockCnbConfiguration, string url = "https://www.cnb.cz/", int retry = 3, int cacheTtl = 5)
		{
			mockCnbConfiguration.Setup(s => s.Value).Returns(new CnbClientConfiguration()
			{
				Retry = retry,
				CacheTtl = cacheTtl,
				CnbTxtClient = new ClientConfiguration
				{
					Url = url
				},
				CnbXmlClient = new ClientConfiguration
				{
					Url = url
				}
			});
		}
	}
}
