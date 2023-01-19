using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface IExchangeRateAccessor
    {
        IEnumerable<ExchangeRate> GetExchangeRates();
    }
}
