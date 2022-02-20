using Common.Exceptions;
using Common.Helper;
using Common.Interface;
using Common.Logs;
using Common.Model;

namespace Common.Providers
{
	/// <summary>
	/// ČNB exchange rate provider. The provider only support CZK currency as a base. All results are sell rates for CZK currency.
	/// </summary>
	public class CnbProvider : IExchangeRateProvider
	{
		#region Fields

		private readonly string _webAddress = "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date={0}.{1}.{2}";

		#endregion

		#region Public methods

		///<inheritDoc/>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
		{
			return await GetExchangeRatesAsync(currencies, DateTime.Now);
		}

		///<inheritDoc/>
		public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies, DateTime dateTime)
		{
			List<Currency> currenciesList = currencies.ToList();
			if (!currenciesList.Any())
				return Enumerable.Empty<ExchangeRate>();

			try
			{
				// Get response from CNB web page.
				string requestAddress = string.Format(_webAddress, dateTime.Day, dateTime.Month, dateTime.Year);

				Log.Instance.Debug($"Getting information from Cnb, request address: {requestAddress}");

				HttpResponseMessage response = await HttpClientHelper.Instance.HttpClient.GetAsync(requestAddress);
				response.EnsureSuccessStatusCode();
				string responseBody = await response.Content.ReadAsStringAsync();

				Log.Instance.Debug("Information successfully received.");
				
				IEnumerable<ExchangeRate> rates = new List<ExchangeRate>();
				// In case of CNB provider there is no point to yield single line, therefore following can be processed all at once.
				await Task.Run(() =>
				{
					CnbParser cnbParser = new CnbParser();
					rates = cnbParser.Parse(responseBody, currenciesList);
				});

				Log.Instance.Info($"Found {rates.Count()} exchange rates");
				return rates;
			}
			catch (Exception e)
			{
				Log.Instance.Error(e.Message);
				// Original exception will be inner exception of wrapping exception.
				throw new GetExcangeRateException(e);
			}
		}

		#endregion
	}
}
