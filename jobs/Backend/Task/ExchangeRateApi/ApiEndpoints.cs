namespace ExchangeRateApi;

public static class ApiEndpoints
{
	public const string ApiVersion = "v1";
	private const string ApiBase = $"{ApiVersion}/api";

	public static class ExchangeRates
	{
		public const string Base = $"{ApiBase}/exchange-rates";
		public const string GetAllByRequestBody = Base;
		public const string GetAllByQueryParams = Base;
	}

	public static class Providers
	{
		public const string Base = $"{ApiBase}/providers";
		public const string GetAll = Base;
	}
}
