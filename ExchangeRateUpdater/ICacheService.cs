using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
	/// <summary>
	/// Cache service interface
	/// </summary>
	public interface ICacheService
	{
		/// <summary>
		/// Gets cached value
		/// </summary>
		/// <typeparam name="T">Generic reference type of a value</typeparam>
		/// <param name="key">A key of value in cache</param>
		/// <param name="value">A value from a cache</param>
		/// <returns>true - if a value was returned from a cache</returns>
		bool TryGetValue<T>(string key, out T value) where T : class;
		/// <summary>
		/// Puts new value to a cache
		/// </summary>
		/// <typeparam name="T">Generic reference type of a value</typeparam>
		/// <param name="key">A key of value in cache</param>
		/// <param name="value">A value</param>
		void SetValue<T>(string key, T value) where T : class;
	}
}
