using System.Xml.Serialization;

namespace ExchangeRate.Client.Cnb.Models.Xml
{
	[XmlRoot(ElementName = "radek")]
	public class XmlExchangeRateRow
	{
		[XmlAttribute(AttributeName = "kod")]
		public string? Code { get; set; }

		[XmlAttribute(AttributeName = "mena")]
		public string? CurrencyName { get; set; }

		[XmlAttribute(AttributeName = "mnozstvi")]
		public int Amount { get; set; }

		[XmlIgnore]
		public decimal Rate { get; set; }

		[XmlAttribute(AttributeName = "kurz")]
		public string RateFormatted
		{
			get => Rate.ToString(CnbConstants.RateFormat);
			set => Rate = decimal.Parse(value, CnbConstants.RateFormat);
		}

		[XmlAttribute(AttributeName = "zeme")]
		public string? Country { get; set; }
	}
}
