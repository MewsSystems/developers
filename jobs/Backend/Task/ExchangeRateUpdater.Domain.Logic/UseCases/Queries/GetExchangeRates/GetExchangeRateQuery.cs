using ExchangeRateUpdater.Domain.Core.Clients;
using ExchangeRateUpdater.Domain.Core.UseCases.Queries.GetExchangeRates;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Logic.UseCases.Queries.GetExchangeRates
{
	public class GetExchangeRateQuery : IGetExchangeRateQuery
	{
		private readonly ILogger _logger;

		private readonly IBankClient _bankClient;

		public GetExchangeRateQuery(ILogger<GetExchangeRateQuery> logger, IBankClient client)
		{
			_logger = logger;
			_bankClient = client;
		}

		public async Task<GetExchangeRateResponse> ExecuteAsync(GetExchangeRateRequest request)
		{
			var rates = await this._bankClient.GetExchange(request.TargetCurrency, request.Date);
			GetExchangeRateResponse response = new GetExchangeRateResponse
			{
				Rates = rates
			};
			return response;
		}
	}
}
