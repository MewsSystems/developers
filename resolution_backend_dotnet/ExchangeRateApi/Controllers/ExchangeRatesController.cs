using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ExchangeRateApi.Controllers
{
    [ApiController]
    [Route("exchangerates")]
    public class ExchangeRatesController : ControllerBase
    {
        // [HttpGet]
        // public async Task<IActionResult> GetRates()
        // {
        //     var provider = new CnbExchangeRateProvider();
        //     var rates = await provider.FetchRatesAsync();
        //     return Ok(rates);
        // }
		[HttpGet]
		public async Task<IActionResult> GetRates()
		{
			var provider = new CnbExchangeRateProvider();
			var rates = await provider.FetchRatesAsync();

			// Simplify the response
			var simplifiedRates = rates.ToDictionary(
				r => r.Code!,
				r => Math.Round(r.Rate / r.Amount, 3)  // Normalize per 1 unit
			);

			var result = new
			{
				base_currency = "CZK",
				rates = simplifiedRates
			};

			return Ok(result);
		}
    }

    public class ExchangeRate
    {
        public string? Country { get; set; }
        public string? Currency { get; set; }
        public int Amount { get; set; }
        public string? Code { get; set; }
        public decimal Rate { get; set; }
    }

    public class CnbExchangeRateProvider
    {
        private const string CnbUrl = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt";

        public async Task<List<ExchangeRate>> FetchRatesAsync()
        {
            using var httpClient = new HttpClient();
            var rawData = await httpClient.GetStringAsync(CnbUrl);
            var lines = rawData.Split('\n').Skip(2); // skip header

            var rates = new List<ExchangeRate>();

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('|');
                var rate = new ExchangeRate
                {
                    Country = parts[0],
                    Currency = parts[1],
                    Amount = int.Parse(parts[2]),
                    Code = parts[3],
                    Rate = decimal.Parse(parts[4], CultureInfo.InvariantCulture)
                };
                rates.Add(rate);
            }

            return rates;
        }
    }
}
