namespace ExchangeRateUpdater.Domain.ValueObjects
{
    public class Currency(string code)
    {

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        private string Code { get; } = code;
        public override string ToString() => Code;
     
    }
}
