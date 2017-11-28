namespace CeskaNarodniBanka.Http {
	using System.Collections.Generic;
	using System.Xml.Serialization;

	[XmlType(TypeName = "tabulka", Namespace = "")]
	public class CnbExchangeRateCollection : List<CnbExchangeRate> { 
	}
}
