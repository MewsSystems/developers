namespace ExchangeRateUpdater.BusinessLogic.Models
{
    public class Currency
    {
        public Currency(string code)
        {
            if (code.Length != 3) throw new ArgumentException($"{nameof(code)} must be a 3 letters code, and {code} is not.", nameof(code));
            Code = code.ToUpperInvariant();
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
