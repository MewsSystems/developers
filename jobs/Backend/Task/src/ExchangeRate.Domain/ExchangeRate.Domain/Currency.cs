namespace ExchangeRate.Domain;

public class Currency(string code)
{
    /// <summary> Three-letter ISO 4217 code of the currency. </summary>
    public string Code { get; } = code;
    
    public override string ToString() => Code;

    public static bool operator ==(Currency left, Currency right) => 
        string.Equals(left.Code, right.Code, StringComparison.OrdinalIgnoreCase);

    public static bool operator !=(Currency left, Currency right) => !(left == right);
}