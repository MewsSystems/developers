using ExchangeRateUpdater.Structures;

namespace ExchangeRateUpdater.Html
{
    public record StringElement(string Value) : ValueObject<string>(Value)
    {

    }
}