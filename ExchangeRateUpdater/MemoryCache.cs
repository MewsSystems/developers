using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SRC = System.Runtime.Caching;

namespace ExchangeRateUpdater
{
	/// <summary>
	/// Cache in memory
	/// </summary>
	internal class MemoryCache : ICacheService
	{
		public void SetValue<T>(string key, T value) where T : class
		{
			SRC.MemoryCache.Default.Add(key, value, DateTime.Today.AddHours(24));
		}

		public bool TryGetValue<T>(string key, out T value) where T : class
		{
			value = SRC.MemoryCache.Default.Get(key) as T;
			return value != null;
		}
	}
}
