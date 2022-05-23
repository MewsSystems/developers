using System.Net;
using ExchangeRate.Client.Cnb.Models.Txt;
using ExchangeRate.Client.Cnb.Models.Xml;

namespace ExchangeRate.UnitTests
{
	public static class TestConstants
	{
		public const string TxtStringOkResponse = @"20.04.2022 #80
země|měna|množství|kód|kurz
EMU|euro|1|EUR|24,450
USA|dolar|1|USD|22,515";

		public const string TxtStringWrongResponse = @"20.04.2022 #80
země|měna|množství|kód|kurz
EMU|1|euro|EUR|24,450
USA|1|dolar|USD|22,515";

		public static readonly HttpResponseMessage HttpResponseMessageTxtExample = new(HttpStatusCode.OK)
		{
			Content = new StringContent(TxtStringOkResponse)
		};

		public static readonly HttpResponseMessage HttpResponseMessageParsingErrorTxtExample = new(HttpStatusCode.OK)
		{
			Content = new StringContent(TxtStringWrongResponse)
		};

		public static readonly HttpResponseMessage HttpResponseMessageEmptyResponseContentTxtExample = new(HttpStatusCode.RequestTimeout)
		{
			Content = null
		};

		public static readonly List<TxtExchangeRate> TxtExchangeRateExample = new()
		{
			new TxtExchangeRate("EMU", "euro", 1, "EUR", (decimal)24.450),
			new TxtExchangeRate("USA", "dolar", 1, "USD", (decimal)22.515)
		};

		public const string XmlStringOkResponse = @"<?xml version=""1.0"" encoding=""UTF-8""?><kurzy banka=""CNB"" datum=""20.04.2022"" poradi=""80""><tabulka typ=""XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU""><radek kod=""EUR"" mena=""euro"" mnozstvi=""1"" kurz=""24,450"" zeme=""EMU""/><radek kod=""USD"" mena=""dolar"" mnozstvi=""1"" kurz=""22,515"" zeme=""USA""/></tabulka></kurzy>";

		public static readonly HttpResponseMessage HttpResponseMessageXmlExample = new(HttpStatusCode.OK)
		{
			Content = new StringContent(XmlStringOkResponse)
		};

		public static readonly XmlExchangeRate XmlExchangeRateExample = new()
		{
			Table = new()
			{
				Rows = new()
				{
					new()
					{
						Code = "EUR",
						CurrencyName = "euro",
						Amount = 1,
						Rate = (decimal)24.450,
						Country = "EMU"
					},
					new()
					{
						Code = "USD",
						CurrencyName = "dolar",
						Amount = 1,
						Rate = (decimal)22.515,
						Country = "USA"
					}
				},
				Type = "XML_TYP_CNB_KURZY_DEVIZOVEHO_TRHU"
			},
			Bank = "CNB",
			Date = Convert.ToDateTime("20.04.2022"),
			OrderNo = 80
		};

		public static readonly XmlExchangeRate XmlExchangeRateNotCompleteExample = new()
		{
			Table = new(),
			Bank = "CNB",
			Date = Convert.ToDateTime("20.04.2022"),
			OrderNo = 80
		};
	}
}
