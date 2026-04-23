namespace ExchangeRateUpdater.Application.Models
{
    public class ExchangeRateModel
    {
        public ExchangeRateModel()
        {
            ExchangeRates = new List<string>();
        }

        public IList<string> ExchangeRates { get; }

        public void Add(string value)
        {
            ExchangeRates.Add(value);
        }
    }
}
