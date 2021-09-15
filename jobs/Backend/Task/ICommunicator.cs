using System;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    public interface ICommunicator
    {
        Task<string> GetExchangeRateData();
        Task<string> GetExchangeRateData(DateTime date);
    }
}