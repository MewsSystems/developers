using ExchangeRateUpdater.Core.Clients.CNB;
using ExchangeRateUpdater.Core.Exceptions;
using ExchangeRateUpdater.Core.UseCases.CommonModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Logic.Clients.CNB
{
	public partial class CzechNationalBankService : ICzechNationalBankService
	{
		private const string _bankCurrency = "CZK";

		private readonly HttpClient _client;

		private readonly ILogger _logger;

		public CzechNationalBankService(ILogger<CzechNationalBankService> logger, HttpClient client)
		{
			_logger = logger;
			_client = client;
		}

		public async Task<IEnumerable<ExchangeRate>> GetExchange(string targetCurrency, DateTime? dateTime)
		{
			StringBuilder url = new StringBuilder($"exrates/daily-currency-month");
			url.Append($"?currency={targetCurrency}");

			if (dateTime.HasValue)
			{
				url.Append($"&url&yearMonth={dateTime.Value.ToString("yyyy-MM")}");
			}

			var response = await _client.GetAsync(url.ToString());
			if (!response.IsSuccessStatusCode)
			{
				throw new DomainException(System.Net.HttpStatusCode.BadGateway, $"Invalid response from {nameof(CzechNationalBankService)} client {response.StatusCode}.");
			}

			var body = await response.Content.ReadAsStringAsync();

			Response data = await response.Content.ReadFromJsonAsync<Response>();
			return data.Rates.Select(e => new ExchangeRate(
				_bankCurrency,
				targetCurrency,
				DateTime.Parse(e.ValidFor),
				e.Rate
				));
		}
	}
}