namespace ExchangeRateUpdater.Chain_of_Responsibility
{
    public abstract class Handler : IHandler
    {
        protected IHandler? next;

        public void SetNext(Handler next) => this.next = next;

        public abstract ExchangeRate GetExchangeRate(Currency currency);
    }
}
