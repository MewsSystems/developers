namespace ExchangeRateUpdater
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code;
        }

        public string Code { get; }

        public override string ToString()
        {
            return Code;
        }
    }
}
