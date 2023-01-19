using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IExchangeRateProvider
    {
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> Currencies);
    }
}
