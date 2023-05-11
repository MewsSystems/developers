using ExchangeRateUpdater.DAL.Models;

namespace ExchangeRateUpdater.DAL.Interfaces
{
    public interface IDataScrapper
    {
        List<RateModel> GetRatesFromWeb(string URL); 
    }
}
