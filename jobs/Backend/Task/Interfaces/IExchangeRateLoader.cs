using System.IO;
using System.Threading.Tasks;

namespace ExchangeRateUpdater.Interfaces;

public interface IExchangeRateLoader
{
    public Task<Stream> ReadAsync();
}