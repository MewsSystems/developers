using Common.Results;
using Entities.Dtos;
using Entities.Concrete;

namespace Business.Abstract
{
    public interface IExchangeRateProviderService
    {
        IDataResult<IEnumerable<ExchangeRate>> GetExchangeRates(CurrencyListRecord currencyListRecord);
    }
}
