namespace ExchangeRateUpdater.Domain.Types;

public class NonEmptyList<T>
{
    public List<T> Values { get; }

    private NonEmptyList(IEnumerable<T> values)
    {
        Values = values.ToList();
    }

    public static NonEmptyList<T>? Create(IEnumerable<T> values)
    {
        return values!=null && values.Any() ? new NonEmptyList<T>(values) : null;
    }

    public static NonEmptyList<T> CreateUnsafe(IEnumerable<T> values)
    {
        if (values == null)
        {
            throw new ArgumentException("You cannot create a non empty list from null");
        }       
        if (!values.Any())
        {
            throw new ArgumentException("You cannot create a non empty list from an empty list");
        }

        return new NonEmptyList<T>(values);
    }
}