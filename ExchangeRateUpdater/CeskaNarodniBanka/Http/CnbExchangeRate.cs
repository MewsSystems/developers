namespace CeskaNarodniBanka.Http {
	using System.Xml.Serialization;

	[XmlType(TypeName = "radek", Namespace = "")]
	public class CnbExchangeRate {
		[XmlAttribute(AttributeName = "kod", Namespace = "")]
		public string Code { get; set; }
		[XmlAttribute(AttributeName = "kurz", Namespace = "")]
		public string Value { get; set; }
	}
}