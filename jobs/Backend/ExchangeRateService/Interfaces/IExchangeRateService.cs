namespace CurrencyExchangeService.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExchangeRateService<E, C>
    {
        Task<IEnumerable<E>> GetExchangeRatesAsync(IEnumerable<C> currencies);
    }
}
