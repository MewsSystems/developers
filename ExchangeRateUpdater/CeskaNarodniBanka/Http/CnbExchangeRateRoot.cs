namespace CeskaNarodniBanka.Http {
	using System.Xml.Serialization;

	[XmlRoot(ElementName = "kurzy", Namespace = "")]
	public class CnbExchangeRateRoot {
		[XmlArray("tabulka", Namespace = "", Order = 0)]
		public CnbExchangeRateCollection ExchangeRates { get; set; }
	}
}
