using System.Collections.Generic;

namespace ExchangeRateUpdater.Helpers
{
    public interface IReadOnlyRepository<T>
    {
        IEnumerable<T> GetAll();
    }
}
