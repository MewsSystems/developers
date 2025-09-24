namespace ExchangeRateUpdater.Core.Common;

public readonly struct Maybe<T>
{
    private readonly T? _value;
    private readonly bool _hasValue;

    private Maybe(T value)
    {
        _value = value;
        _hasValue = true;
    }

    public static Maybe<T> Nothing
    {
        get => default;
    }

    public static implicit operator Maybe<T>(T? value) => value != null ? new Maybe<T>(value) : Nothing;

    public bool HasValue => _hasValue;

    /// <summary>
    /// Gets the value if it exists, otherwise throws an exception
    /// </summary>
    public T Value => _hasValue ? _value! : throw new InvalidOperationException("Maybe has no value");


    public T GetValueOrDefault(T defaultValue = default!)
    {
        return _hasValue ? _value! : defaultValue;
    }

    public bool TryGetValue(out T value)
    {
        value = _hasValue ? _value! : default!;
        return _hasValue;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Maybe<T> other)
        {
            if (!_hasValue && !other._hasValue)
                return true;
            if (_hasValue && other._hasValue)
                return EqualityComparer<T>.Default.Equals(_value, other._value);
        }
        return false;
    }

    public static bool operator ==(Maybe<T> left, Maybe<T> right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Maybe<T> left, Maybe<T> right)
    {
        return !left.Equals(right);
    }

    public override int GetHashCode()
    {
        return _hasValue ? EqualityComparer<T>.Default.GetHashCode(_value!) : 0;
    }
}