namespace Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IReadOnlyCollection<T> ToReadOnlyCollection<T>(this IEnumerable<T> source)
        {
            return source.ToList();
        }
    }
}