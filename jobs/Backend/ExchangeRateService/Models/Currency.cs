namespace ExchangeRateService.Models
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

        public override string ToString()
        {
            return Code;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || !(obj is Currency))
            {
                return false;
            }
            return ((Currency)obj).Code == this.Code;
        }

        public override int GetHashCode()
        {
            return this.Code.GetHashCode();
        }
    }
}
