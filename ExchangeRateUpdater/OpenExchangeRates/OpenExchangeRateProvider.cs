namespace OpenExchangeRates {
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;
	using ExchangeRateUpdater.Financial;
	using OpenExchangeRates.Http;

	public sealed class OpenExchangeRateProvider : ExchangeRateProvider<IOpenExchangeRateProviderOptions> {
		public OpenExchangeRateProvider(IOpenExchangeRateProviderOptions options)
			: base(options) {
			AppId = options.AppId;
		}

		public AppId AppId { get; }

		protected override string CreateRequestUriString(IEnumerable<Currency> currencies) {
			var sb = currencies.Aggregate(new StringBuilder($"latest.json?app_id={AppId}&symbols="), (feed, item) => feed.AppendFormat("{0},", item));
			sb.Remove(sb.Length - 1, 1);

			string requestUriString = sb.ToString();

			return requestUriString;
		}

		protected override async Task<IEnumerable<ExchangeRate>> GetExchangeRateCoreAsync(IEnumerable<Currency> currencies) {
			var requestUriString = CreateRequestUriString(currencies);

			var temp = await Client.GetAsync<OpenExchangeRate>(requestUriString);

			var result = temp.Rates
				.Where(r => temp.Base != r.Key)
				.Select(r => new ExchangeRate(new Currency(temp.Base), new Currency(r.Key), r.Value));

			return result;
		}
	}
}