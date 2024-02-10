using ExchangeRateUpdater.Interfaces;

namespace ExchangeRateUpdater.Models
{
    public class Currency : ICurrency
    {
        public Currency(string code)
        {
            Code = code;
        }

        public string Code { get; }

        public override string ToString() => Code;
    }
}
