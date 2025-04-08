using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net.Http;
using System.Globalization;

namespace ExchangeRateUpdater
{
	public class ExchangeRateProvider
	{
		/// <summary>
		/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
		/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
		/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
		/// some of the currencies, ignore them.
		/// </summary>

		static private string OpenFile(string path)
		{
			if (File.Exists(path) == false)
			{
				throw new Exception("invalid file: file does not exist");
			}
			string content = File.ReadAllText(path);
			return content;
		}
		static IEnumerable<ExchangeRate> parseContent(string content)
		{
			var rates = new List<ExchangeRate>();
			string[] entries = content.Split('\n');

			if (entries.Length <= 1)
			{
				throw new Exception("invalid rate syntax: less than 2 entries");
			}

			string[] header = entries[0].Split('|');
			string[] body = entries[1].Split('|');

			if (header.Length != body.Length)
			{
				throw new Exception("invalid rate syntax: header length is not equal to body length");
			}
			for (int i = 1; i < body.Length; i++)
			{
				try
				{
					string[] splitHeader = header[i].Split(' '); // splitHeader[0] = Currency | splitHeader[1] = count
					if (splitHeader.Length != 2)
						throw new Exception($"\" {header[i]} \" contains more than 1 space");

					string sourceCurrency = "CZK";
					string targetCurrency = splitHeader[1];
					decimal rate = (decimal)float.Parse(splitHeader[0]) * (decimal)float.Parse(body[i]); // convert to decimal later because decimal.Parse keeps breaking :))

					rates.Add(new ExchangeRate(sourceCurrency, targetCurrency, rate));
				}
				catch (Exception e)
				{
					throw new Exception($"invalid rate syntax @ \" {header[i]} ~ {body[i]}\"\n {e.Message}");
				}
			}
			return rates;
		}

		public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
		{
			try
			{
				string content = OpenFile("rates.txt");
				IEnumerable<ExchangeRate> rates = parseContent(content);
				return rates;

			}
			catch (Exception e)
			{
				Console.Error.WriteLine(e);
			}
			return Enumerable.Empty<ExchangeRate>();
		}
	}
}
