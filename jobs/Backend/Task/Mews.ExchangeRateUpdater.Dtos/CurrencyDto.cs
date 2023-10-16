namespace Mews.ExchangeRateUpdater.Dtos
{
    public class CurrencyDto
    {
        public CurrencyDto(string code)
        {
            Code = code;
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
