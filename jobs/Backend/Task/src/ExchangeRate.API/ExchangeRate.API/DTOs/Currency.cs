namespace ExchangeRate.API.DTOs;

//Note: the UI layer models are are independent of the domain models which improves maintainability and flexibility
public class Currency(string code)
{
    /// <summary> Three-letter ISO 4217 code of the currency. </summary>
    public string Code { get; } = code;
    
    public override string ToString() => Code;
}