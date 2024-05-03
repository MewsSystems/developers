using ExchangeRateUpdater.Domain.Core.Clients;
using ExchangeRateUpdater.Domain.Core.UseCases.CommonModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Domain.Logic.Clients
{
	public class CzechNationalBankClient : IBankClient
	{
		private static readonly Currency _bankCurrency = new Currency("CZK");

		private readonly ILogger _logger;

		private readonly IHttpBankClientWrapper _client;

		public CzechNationalBankClient(ILogger<CzechNationalBankClient> logger, IHttpBankClientWrapper client)
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

			Currency target = new Currency(targetCurrency);

			Response data = await response.Content.ReadFromJsonAsync<Response>();
			return data.Rates.Select(e => new ExchangeRate(
				_bankCurrency,
				target,
				DateTime.Parse(e.ValidFor),
				e.Rate
				));
		}

		public class CurrencyRate
		{
			public int Amount { get; set; }

			public string Country { get; set; }

			public string Currency { get; set; }

			public string CurrencyCode { get; set; }

			public int Order { get; set; }

			public decimal Rate { get; set; }

			public string ValidFor { get; set; }
		}

		public class Response
		{
			public IEnumerable<CurrencyRate> Rates { get; set; }
		}
	}
}
