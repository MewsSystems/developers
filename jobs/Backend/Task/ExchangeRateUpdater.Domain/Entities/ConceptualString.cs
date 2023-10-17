namespace ExchangeRateUpdater.Domain.Entities;

public abstract class ConceptualString
{
    public abstract string Value { get; }

    public override string ToString() => Value;
    public override int GetHashCode() => Value.GetHashCode();

    public static implicit operator string(ConceptualString conceptualString) => conceptualString?.Value ?? string.Empty;

    public static bool operator ==(ConceptualString d1, ConceptualString d2)
    {
        if ((object)d1 == null)
            return (object)d2 == null;

        return d1.Equals(d2);
    }

    public static bool operator !=(ConceptualString d1, ConceptualString d2) => !(d1 == d2);

    public override bool Equals(object? obj)
    {
        if (Value == null || obj == null)
            return false;

        if (obj is string str)
        {
            return string.Equals(Value, str, StringComparison.Ordinal);
        }

        if (obj.GetType() != this.GetType())
            return false;

        string objStr = $"{obj}";

        return string.Equals(Value, objStr, StringComparison.Ordinal);
    }
}