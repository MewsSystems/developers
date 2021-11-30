using System.Collections.Generic;
using System.Xml.Serialization;

namespace ExchangeRateUpdater.Models
{
    [XmlRoot(ElementName="kurzy")]
	public class CnbModel {
		[XmlElement(ElementName="tabulka")]
		public Tabulka Tabulka { get; set; }
	}

	[XmlRoot(ElementName="tabulka")]
	public class Tabulka {
		[XmlElement(ElementName="radek")]
		public List<Radek> Radek { get; set; }
	}

    [XmlRoot(ElementName="radek")]
	public class Radek {
		[XmlAttribute(AttributeName="kod")]
		public string Kod { get; set; }

		[XmlAttribute(AttributeName="mnozstvi")]
		public string Mnozstvi { get; set; }

		[XmlAttribute(AttributeName="kurz")]
		public string Kurz { get; set; }
	}
}
