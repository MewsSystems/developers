namespace ExchangeRateUpdater.Domain.Types
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = ValidCode.Create(code);
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public ValidCode? Code { get; }

        public override string? ToString()
        {
            return Code?.Value;
        }
    }
}
