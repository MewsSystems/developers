namespace ExchangeRateUpdater
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
        public string Code { get; private set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Currency))
                return false;

            return Code == ((Currency)obj).Code;
        }
    }
}
