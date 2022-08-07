namespace CurrencyExchangeService.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public interface IExchangeRateProvider<T>
    {
       Task<T> GetExchangeRates();
    }
}
