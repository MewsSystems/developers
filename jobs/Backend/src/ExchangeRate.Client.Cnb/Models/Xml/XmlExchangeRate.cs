using System.Globalization;
using System.Xml.Serialization;

namespace ExchangeRate.Client.Cnb.Models.Xml
{
	[XmlRoot(ElementName = "kurzy")]
	public class XmlExchangeRate
	{
		[XmlElement(ElementName = "tabulka")]
		public XmlExchangeRateTable? Table { get; set; }

		[XmlAttribute(AttributeName = "banka")]
		public string? Bank { get; set; }

		[XmlIgnore]
		public DateTime Date { get; set; }

		[XmlAttribute("datum")]
		public string DateFormatted
		{
			get => Date.ToString(CnbConstants.DateFormat, CultureInfo.InvariantCulture);
			set => Date = DateTime.ParseExact(value, CnbConstants.DateFormat, CultureInfo.InvariantCulture);
		}

		[XmlAttribute(AttributeName = "poradi")]
		public int OrderNo { get; set; }
	}
}
