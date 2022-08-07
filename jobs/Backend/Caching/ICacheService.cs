namespace Caching
{
    public interface ICacheService<K, V>
    {
        void Add(K key, V value, DateTimeOffset expire);
        V Get(K key);
        void Remove(K key);
        void Clear();

    }
}
