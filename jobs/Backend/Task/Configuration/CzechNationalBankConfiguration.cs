using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace ExchangeRateUpdater.Configuration
{
    public interface ICzechNationalBankConfiguration
    {
        public string ExchangeRateUri();
        public string CSVResponseDelimiter();
        public bool CSVExpectingHeaders();
    }

    public class CzechNationalBankConfiguration : ICzechNationalBankConfiguration
    {
        private readonly IConfiguration _configuration;

        public CzechNationalBankConfiguration(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("CzechNationalBank");
        }

        public string ExchangeRateUri()
        {
            return _configuration.GetValue<string>("ExchangeRatesUri");
        }
        public string CSVResponseDelimiter()
        {
            return _configuration.GetValue<string>("CSVResponseDelimiter");
        }
        public bool CSVExpectingHeaders()
        {
            return _configuration.GetValue<bool>("CSVExpectingHeaders");
        }
    }
}