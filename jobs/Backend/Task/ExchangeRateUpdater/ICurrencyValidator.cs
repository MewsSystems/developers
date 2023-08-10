namespace ExchangeRateUpdater;

public interface ICurrencyValidator
{
    bool IsValid(string code);
}

class CurrencyValidator : ICurrencyValidator
{
    // Todo contains valid code from cnb api
    public bool IsValid(string code)
    {
        return code.Length == 3 && code.All(char.IsLetter);
    }
}