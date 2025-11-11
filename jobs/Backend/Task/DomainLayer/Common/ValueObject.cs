namespace DomainLayer.Common;

/// <summary>
/// Base class for value objects.
/// Value objects are immutable and compared by their values rather than identity.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    protected abstract IEnumerable<object?> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;
        return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public bool Equals(ValueObject? other)
    {
        return Equals((object?)other);
    }

    public override int GetHashCode()
    {
        var components = GetEqualityComponents().ToArray();

        if (components.Length == 0)
            return 0;

        var hash = new HashCode();
        foreach (var component in components)
        {
            hash.Add(component);
        }

        return hash.ToHashCode();
    }

    public static bool operator ==(ValueObject? left, ValueObject? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(ValueObject? left, ValueObject? right)
    {
        return !Equals(left, right);
    }
}
