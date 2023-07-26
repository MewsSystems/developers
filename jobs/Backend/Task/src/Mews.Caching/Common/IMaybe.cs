namespace Mews.Caching.Common
{
    public interface IMaybe<out T>
    {
        bool HasValue { get; }

        T Value { get; }
    }
}
