using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ExchangeRateUpdater.Structures
{
    public interface ITableRow
    {
        ITableRow Create(ReadOnlyCollection<string> headerNames, IEnumerable<string> values);
    }
}