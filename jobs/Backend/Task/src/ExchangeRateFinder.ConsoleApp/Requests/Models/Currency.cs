namespace ExchangeRateFinder.ConsoleApp.Requests.Models
{
    public class Currency
    {
        public Currency(string code)
        {
            Code = code;
        }

        public string Code { get; }
    }
}
