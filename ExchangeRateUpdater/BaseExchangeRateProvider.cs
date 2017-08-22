using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net;

namespace ExchangeRateUpdater
{
	/// <summary>
	/// The base class of exchange rate provider
	/// </summary>
	public abstract class BaseExchangeRateProvider : IExchangeRateProvider
	{
		private static readonly object _lock = new object();

		private string _appSettingsName;
		private string _sourceUrl;
		private ICacheService _cacheService;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="appSettingsName">Name in appSettings section which includes URL of exchange rates' source provides by a bank</param>
		/// <param name="cacheService">Service for caching source data</param>
		public BaseExchangeRateProvider(string appSettingsName, ICacheService cacheService)
		{
			_appSettingsName = appSettingsName;
			_cacheService = cacheService;

			Logger.Trace(string.Format("Exchange rate provider of type {0} has been created", this.GetType().Name));
		}

		public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			if (currencies == null)
				throw new ArgumentNullException("currencies");

			if (string.IsNullOrEmpty(_sourceUrl))
			{
				lock (_lock)
				{
					if (string.IsNullOrEmpty(_sourceUrl))
					{
						Logger.Trace(string.Format("Initializing of {0}", this.GetType().Name));

						if (string.IsNullOrEmpty(_appSettingsName))
							throw new ArgumentNullException("appSettingsName", "Name of the required appSettings section wasn't provided");

						_sourceUrl = ConfigurationManager.AppSettings[_appSettingsName];
						if (string.IsNullOrEmpty(_sourceUrl))
							throw new ArgumentNullException("appSettingsName", "Required appSettings section wasn't provided");
					}
				}
			}

			Dictionary<string, ExchangeRate> rates;
			if (!_cacheService.TryGetValue("lastRates", out rates))
			{
				lock (_lock)
				{
					if (!_cacheService.TryGetValue("lastRates", out rates))
					{
						var response = DownloadData();
						try
						{
							rates = ParseResponce(response);
						}
						catch (Exception ex)
						{
							Logger.Trace("Parsing response's data error: " + ex.Message);
							throw new ExchangeRateProviderException("Can't parse data of response", ex);
						}
						_cacheService.SetValue("lastRates", rates);
					}
				}
			}

			var listRates = new List<ExchangeRate>();
			foreach(var curr in currencies)
			{
				ExchangeRate rate;
				if (rates.TryGetValue(curr.Code, out rate))
					listRates.Add(rate);
			}

			Logger.Trace(string.Format("{0} exchange rates have been got", listRates.Count));

			return listRates;
		}

		/// <summary>
		/// Parses source's data. Has to be implemented for certain provider
		/// </summary>
		/// <param name="response">Data from a bank source</param>
		/// <returns></returns>
		protected abstract Dictionary<string, ExchangeRate> ParseResponce(string response);

		private string DownloadData()
		{
			Logger.Trace(string.Format("Data will be downloaded from: {0}", _sourceUrl));

			using (var webClient = new WebClient())
			{
				try
				{
					webClient.Encoding = Encoding.UTF8;
					return webClient.DownloadString(_sourceUrl);
				}
				catch(Exception ex)
				{
					Logger.Trace("Downloading data error: " + ex.Message);
					throw new ExchangeRateProviderException("Downloading data error", ex);
				}
			}
		}
	}
}