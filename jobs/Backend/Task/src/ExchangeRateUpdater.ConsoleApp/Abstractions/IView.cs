namespace ExchangeRateUpdater.ConsoleApp.Abstractions
{
    public interface IView
    {
        void WriteLine(string output);
        void LineFeed();
    }
}
