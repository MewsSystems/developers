using ExchangeRateUpdater.ConsoleApp.Abstractions;

namespace ExchangeRateUpdater.ConsoleApp
{
    public class View : IView
    {
        public void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
        public void LineFeed()
        {
            Console.WriteLine(string.Empty);
        }
    }
}
