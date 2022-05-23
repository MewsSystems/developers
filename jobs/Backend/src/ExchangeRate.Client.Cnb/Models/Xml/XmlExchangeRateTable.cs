using System.Xml.Serialization;

namespace ExchangeRate.Client.Cnb.Models.Xml
{
	[XmlRoot(ElementName = "tabulka")]
	public class XmlExchangeRateTable
	{
		[XmlElement(ElementName = "radek")]
		public List<XmlExchangeRateRow>? Rows { get; set; }

		[XmlAttribute(AttributeName = "typ")]
		public string? Type { get; set; }
	}
}
