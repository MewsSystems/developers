using ExchangeRateLayer.BLL.Objects;

namespace ExchangeRateLayer.BLL.Services.Abstract
{
    public interface IExchangeRateService
    {
        List<ExchangeRate> GetAllExchangeRates();

        IEnumerable<ExchangeRate> GetSelectedExchangeRates(IEnumerable<Currency> currencies);
    }
}