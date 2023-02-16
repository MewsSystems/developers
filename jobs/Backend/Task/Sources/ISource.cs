using System.Threading.Tasks;

namespace ExchangeRateUpdater.Sources;

public interface ISource
{
    Task<string> GetContent();
}
