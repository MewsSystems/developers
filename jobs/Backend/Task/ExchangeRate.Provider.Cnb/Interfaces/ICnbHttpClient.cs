using ExchangeRate.Provider.Cnb.Models;

namespace ExchangeRate.Provider.Cnb.Interfaces;

public interface ICnbHttpClient
{
    Task<List<CnbExchangeRate>> GetExchangeRate();
}