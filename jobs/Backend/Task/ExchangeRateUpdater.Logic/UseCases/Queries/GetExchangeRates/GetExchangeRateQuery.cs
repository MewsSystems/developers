using ExchangeRateUpdater.Core.Clients;
using ExchangeRateUpdater.Core.Clients.CNB;
using ExchangeRateUpdater.Core.UseCases.Queries.GetExchangeRates;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Logic.UseCases.Queries.GetExchangeRates
{
	/// <summary>
	/// Implementation for the get exchange query.
	/// </summary>
	public class GetExchangeRateQuery : IGetExchangeRateQuery
	{
		private readonly ILogger _logger;

		private readonly IBankService _bankClient;

		public GetExchangeRateQuery(ILogger<GetExchangeRateQuery> logger, ICzechNationalBankService bankService)
		{
			_logger = logger;
			_bankClient = bankService;
		}

		/// <inheritdoc/>
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
