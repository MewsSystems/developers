using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Mews.ExchangeRate.Storage.Abstractions.Models
{
    /// <summary>
    /// This class holds the logic to get the storage state.
    /// </summary>
    public class StorageState : ConcurrentDictionary<string, StorageStatus>
    {
    }
}
