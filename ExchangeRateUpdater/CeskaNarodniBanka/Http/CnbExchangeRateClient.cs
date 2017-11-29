namespace CeskaNarodniBanka.Http {
	using System;
	using System.Net.Http;
	using System.Threading.Tasks;
	using System.Xml.Serialization;
	using ExchangeRateUpdater.Financial.Http;

	public class CnbExchangeRateClient : HttpExchangeRateClient, ICnbExchangeRateClient {
		public CnbExchangeRateClient(ICnbExchangeRateClientOptions options)
			: base(options) { }

		protected override async Task<TResult> ReadContentAsync<TResult>(HttpContent httpContent) {
			using (var stream = await httpContent.ReadAsStreamAsync()) {
				var serializer = new XmlSerializer(typeof(TResult), String.Empty);

				var result = (TResult)serializer.Deserialize(stream);

				return result;
			}
		}
	}
}
