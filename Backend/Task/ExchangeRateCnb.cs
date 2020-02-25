using System.Globalization;

namespace ExchangeRateUpdater
{
	/*
	 * To implement exchange rate provider in terms of OOP, we need an object which would represent particular CNB exchange rate
	 * so that we can easily add/replace this object with yet another one with specs belonging to that another bank exchange rate format wihout changing our codebase (almost)
	 * also OOP and SOLID helps us avoid spagetti
	 * 
	 * I choose to derive this object from ExchangeRate class as it saves us a mapping when we return from the main defined method (GetExchangeRates) taking into account its signature
	 * If Exchange rate would have been sealed - this class will be standalone.
	 * 
	 * I also slight modified base ExhangeRate class to make it more complient with S O LID which promotes Open for Extension principle 
	 * without yet breaking its signature for other possible class consumers
	 */


	/// <summary>
	/// Country|Currency|Amount|Code|Rate
	/// Australia|dollar|1|AUD|15.466 
	/// </summary>
	public class ExchangeRateCnb : ExchangeRate
	{
		private const string targetCurrencyCode = "CZK";
		private const char separatorField = '|';

		private const int positionCountry = 0;
		private const int positionCurrencyName = 1;
		private const int positionAmount = 2;
		private const int positionCurrencyCode = 3;
		private const int positionRate = 4;

		public ExchangeRateCnb(string exchangeRateLine, string separatorDecimal = ",") 
		{
			TargetCurrency = new Currency(targetCurrencyCode);

			string[] dataArray = exchangeRateLine.Trim().Split(new char[] { separatorField });

			SourceCurrency = new Currency(dataArray[positionCurrencyCode]);

			NumberFormatInfo formatInfo = new NumberFormatInfo
			{
				NumberDecimalSeparator = separatorDecimal
			};

			decimal rate = decimal.Parse(dataArray[positionRate], formatInfo);

			int amount = int.Parse(dataArray[positionAmount]);

			Value = rate / amount;
		}
	}
}
