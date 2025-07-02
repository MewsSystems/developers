using System.Collections.Generic;

namespace ExchangeRateUpdater.Logic.Clients.CNB
{
	public partial class CzechNationalBankService
	{
		public class Response
		{
			public IEnumerable<CurrencyRate> Rates { get; set; }
		}
	}
}