using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Common;

internal interface IOperation
{
    Task ExecuteAsync(IEnumerable<object> args, CancellationToken cancellationToken);
}
