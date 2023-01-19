using Common.CrossCuttingConcerns.Validation;
using DataAccess.Abstract;
using Entities.Models;
using Entities.ValidationRules.FluentValidation;
using Entities.Concrete;
using Microsoft.Extensions.Configuration;
using Common.Results;
using FluentValidation;
using DataAccess.Constants;

namespace DataAccess.Concrete
{
    public class ExchangeRateAccessor : IExchangeRateAccessor
    {
        public IConfiguration Configuration { get; }
        private  string _exchangeRateSourceUrl;
        private string _exchangeRateSourceCurrency;

        public ExchangeRateAccessor(IConfiguration configuration)
        {
            Configuration = configuration;
            _exchangeRateSourceUrl = Configuration.GetSection("ExchangeRateSourceUrl").Value;
            _exchangeRateSourceCurrency = Configuration.GetSection("ExchangeRateSourceCurrency").Value;
        }

        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            CheckUrlAndCurrencySettins(_exchangeRateSourceUrl, _exchangeRateSourceCurrency);            
            var getExchangeRatesFromSourceAsString = getExchangeRatesFromSource().Result;
            var parsedExhangeRates = getExchangeRatesFromParser(getExchangeRatesFromSourceAsString);
           return parsedExhangeRates;   
        }

        private void CheckUrlAndCurrencySettins(string exchangeRateSourceUrl, string exchangeRateSourceCurrency)
        {
            if (string.IsNullOrEmpty(_exchangeRateSourceUrl) )
                throw new ValidationException(Messages.WrongSourceUrlSetting);
            if (string.IsNullOrEmpty(exchangeRateSourceCurrency) )
                throw new ValidationException(Messages.WrongSourceUrlSetting);
        }

        private Task<string> getExchangeRatesFromSource()
        {
            using (var httpClient = new HttpClient())
            {
                var sendExchangeRequestUrl = _exchangeRateSourceUrl;

                using HttpClient client = new HttpClient();
                using HttpResponseMessage response = client.GetAsync(sendExchangeRequestUrl).Result;
                response.EnsureSuccessStatusCode();

                var respAsString = response.Content.ReadAsStringAsync().Result;
                return Task.FromResult(respAsString);
            }
        }

        private IEnumerable<ExchangeRate> getExchangeRatesFromParser(string exchangeRatesAsString)
        {
            var respAsStringSplitted = exchangeRatesAsString.Contains(Environment.NewLine) ?
                   exchangeRatesAsString.Split(Environment.NewLine) :
                   exchangeRatesAsString.Split('\n');

            var CNBExchangeRates = respAsStringSplitted.Skip(2);

            return CNBExchangeRates.
                Select(exchangeRateLine => exchangeRateLine.Split('|')).
                Where(exchangeRateLine => exchangeRateLine.Length >= 5 && decimal.TryParse(exchangeRateLine[4], out _)).
                Select(exchangeRateLineSplitted => new ExchangeRate(new Currency(_exchangeRateSourceCurrency), new Currency(exchangeRateLineSplitted[3]), decimal.Parse(exchangeRateLineSplitted[4]))).ToArray();
        }
    }
}
