using System;
using System.Collections.Generic;

namespace ExchangeRateUpdater
{
	public interface IRateExtractor
    {
		/// <summary>
		/// Exctracts rates from <paramref name="content"/>.
		/// </summary>
		/// <param name="content">The content.</param>
		/// <returns>The collection of code - rate pairs.</returns>
		/// <exception cref="ArgumentNullException"><paramref name="content"/> is null.</exception>
        IDictionary<string, decimal> Extract(string content);
    }
}
