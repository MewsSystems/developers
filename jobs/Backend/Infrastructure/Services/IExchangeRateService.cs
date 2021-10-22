using Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public interface IExchangeRateService
    {
        public Task<IEnumerable<GenericRate>> GetExchangeRatesAsync();
    }
}