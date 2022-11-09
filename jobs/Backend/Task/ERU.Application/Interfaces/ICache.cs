namespace ERU.Application.Interfaces;

public interface ICache
{
	T? GetFromCache<T>(string cacheKey);
	void InsertToCache(string cacheKey, object value);
}