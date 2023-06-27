namespace ExchangeRateUpdater;

public interface ICzechNationalBankExchangeRateGateway
{
    CnbExchangeRates GetCurrentRates();
}