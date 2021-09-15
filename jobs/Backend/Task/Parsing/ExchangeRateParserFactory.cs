namespace ExchangeRateUpdater.Parsing
{
    public class ExchangeRateParserFactory : IExchangeRateParserFactory
    {
        public IExchangeRateParser CreateParser(ExchangeRateParserType type)
        {
            switch (type)
            {
                case ExchangeRateParserType.CNB:
                default:
                    return new CNBExchangeRateParser();
            }
        }
    }
}
