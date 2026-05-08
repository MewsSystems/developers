using System.Threading.Tasks;

namespace ExchangeRateUpdater.Decorator
{
    public abstract class LoadRates : ILoadRates
    {
        protected ILoadRates wrapper;

        protected LoadRates(ILoadRates wrapper) => this.wrapper = wrapper;

        public abstract Task<bool> Load(string data);
    }
}