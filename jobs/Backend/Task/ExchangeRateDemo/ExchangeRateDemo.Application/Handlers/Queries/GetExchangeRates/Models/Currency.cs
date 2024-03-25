namespace ExchangeRateDemo.Application.Handlers.Queries.GetExchangeRates.Models
{
    //Defaults to Czech Republic
    public class Currency(string code = "CZK")
    {
        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; } = code;

        public override string ToString()
        {
            return Code;
        }
    }
}
