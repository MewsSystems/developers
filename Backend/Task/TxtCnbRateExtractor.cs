using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ExchangeRateUpdater
{
	public class TxtCnbRateExtractor: IRateExtractor
    {
		private const int Amount = 3;
		private const int Code = 4;
		private const int Rate = 5;

		/// <summary>
		/// Exctracts rates from <paramref name="content"/>.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns>The collection of code - rate pairs.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="content"/> is null.</exception>
		public IDictionary<string, decimal> Extract(string content)
        {
			if (content == null)
				throw new ArgumentNullException(nameof(content));
            Regex rx = new Regex(@"\n([\p{L}\s]+)\|([\p{L}-\s]+)\|(\d+)\|(\w{3})\|(\d+(,\d+)?)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
            MatchCollection matches = rx.Matches(content);
            var rates = new Dictionary<string, decimal>(matches.Count);
            foreach (Match match in matches)
            {
                int amount = int.Parse(match.Groups[Amount].Value);
                rates.Add(match.Groups[Code].Value, decimal.Parse(match.Groups[Rate].Value, new CultureInfo("cs")) / amount);
            }
            return rates;
        }
    }
}
