namespace ExchangeRateProvider.Contract.Models
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code;
        }

        /// <summary>
        /// Three-letter ISO 4217 code of the currency.
        /// </summary>
        public string Code { get; }

        public override bool Equals(object? obj)
        {
            if(obj == null || obj.GetType() != GetType()) return false;

            return Code.Equals(((Currency)obj).Code);
        }

        public override string ToString()
        {
            return Code;
        }
    }
}
