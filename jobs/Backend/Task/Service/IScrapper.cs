using System.Threading.Tasks;

namespace ExchangeRateUpdater.Service
{
    public interface IScrapper
    {
        public Task<string> GetData(string Url);
    }
}
