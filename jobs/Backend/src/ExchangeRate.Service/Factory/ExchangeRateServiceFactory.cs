using ExchangeRate.Client.Cnb;
using ExchangeRate.Client.Cnb.Abstract;
using ExchangeRate.Service.Abstract;
using ExchangeRate.Service.Service;

namespace ExchangeRate.Service.Factory
{
	public class ExchangeRateServiceFactory : IExchangeRateServiceFactory
	{
		private readonly IExchangeRateClient _exchangeRateClient;

		public ExchangeRateServiceFactory(IExchangeRateClient exchangeRateClient)
		{
			_exchangeRateClient = exchangeRateClient;
		}

		public IConcreteExchangeRateService GetExchangeRateService(CnbConstants.ApiType apiType)
		{
			return apiType switch
			{
				CnbConstants.ApiType.CnbXml => new CnbXmlExchangeRateService(_exchangeRateClient),
				CnbConstants.ApiType.CnbTxt => new CnbTxtExchangeRateService(_exchangeRateClient),
				_ => throw new NotImplementedException($"Not implemented {apiType}")
			};
		}
	}
}
