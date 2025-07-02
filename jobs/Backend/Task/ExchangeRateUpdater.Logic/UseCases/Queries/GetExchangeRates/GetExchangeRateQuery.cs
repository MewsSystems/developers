using ExchangeRateUpdater.Core.Clients;
using ExchangeRateUpdater.Core.Clients.CNB;
using ExchangeRateUpdater.Core.UseCases.Queries.GetExchangeRates;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
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
		private readonly IMemoryCache _cache;

		public GetExchangeRateQuery(ILogger<GetExchangeRateQuery> logger, ICzechNationalBankService bankService, IMemoryCache cache)
		{
			_logger = logger;
			_bankClient = bankService;
			_cache = cache;
		}

		/// <inheritdoc/>
		public async Task<GetExchangeRateResponse> ExecuteAsync(GetExchangeRateRequest request)
		{
			if (_cache.TryGetValue($"{request.TargetCurrency}:{request.Date}", out GetExchangeRateResponse response))
			{
				return response;
			}
			else
			{
				var rates = await this._bankClient.GetExchange(request.TargetCurrency, request.Date);
				response = new GetExchangeRateResponse
				{
					Rates = rates
				};

				_cache.Set($"{request.TargetCurrency}:{request.Date}", response, 
					new MemoryCacheEntryOptions()
					.SetAbsoluteExpiration(TimeSpan.FromHours(1)));

				return response;
			}
		}
	}
}
