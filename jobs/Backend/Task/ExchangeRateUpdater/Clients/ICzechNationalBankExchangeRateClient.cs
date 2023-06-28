using ExchangeRateUpdater.Clients.Models;

namespace ExchangeRateUpdater.Clients;

public interface ICzechNationalBankExchangeRateClient
{
    CnbExchangeRates GetCurrentRates();
}