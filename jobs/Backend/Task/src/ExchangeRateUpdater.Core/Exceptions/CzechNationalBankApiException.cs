namespace ExchangeRateUpdater.Core.Exceptions
{
    public class CzechNationalBankApiException : Exception
    {
        public CzechNationalBankApiException(string message) : base(message)
        { }
    }
}
