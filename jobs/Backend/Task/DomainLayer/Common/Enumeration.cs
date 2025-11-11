using System.Reflection;

namespace DomainLayer.Common;

/// <summary>
/// Base class for creating type-safe enumerations.
/// Provides an alternative to standard enums with more functionality.
/// </summary>
public abstract class Enumeration : IComparable
{
    public int Id { get; private set; }
    public string Name { get; private set; }

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => Name;

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        return fields
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    public static T? FromValue<T>(int value) where T : Enumeration
    {
        return Parse<T, int>(value, "value", item => item.Id == value);
    }

    public static T? FromName<T>(string name) where T : Enumeration
    {
        return Parse<T, string>(name, "name", item => item.Name == name);
    }

    private static T? Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);
        return matchingItem;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = Id.Equals(otherValue.Id);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public int CompareTo(object? other)
    {
        if (other is Enumeration enumeration)
        {
            return Id.CompareTo(enumeration.Id);
        }

        return 1;
    }
}
