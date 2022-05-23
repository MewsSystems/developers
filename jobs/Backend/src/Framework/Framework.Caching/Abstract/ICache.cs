namespace Framework.Caching.Abstract
{
	/// <summary>
	/// In memory cache
	/// </summary>
	public interface ICache
	{
		/// <summary>
		/// Get value from cache
		/// </summary>
		/// <typeparam name="T">Cache value</typeparam>
		/// <param name="key">Cache key</param>
		/// <returns>Cache value</returns>
		T? Get<T>(string key);

		/// <summary>
		/// Set cache value
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key">Cache key</param>
		/// <param name="value">Cache value</param>
		/// <param name="ttl">Cache time to live</param>
		/// <returns>true - if data was set to cache</returns>
		bool Set<T>(string key, T? value, int ttl = 0);
	}
}
