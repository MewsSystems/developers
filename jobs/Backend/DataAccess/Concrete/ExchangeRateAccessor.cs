using Common.CrossCuttingConcerns.Validation;
using DataAccess.Abstract;
using Entities.Models;
using Entities.ValidationRules.FluentValidation;
using ExchangeRateUpdater;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class ExchangeRateAccessor : IExchangeRateAccessor
    {
        public IConfiguration Configuration { get; }
        private readonly ExchangeRateSourceSettings _exchangeRateSourceSettings;

        public ExchangeRateAccessor(IConfiguration configuration)
        {
            Configuration = configuration;
            _exchangeRateSourceSettings = Configuration.GetSection("ExchangeRateSourceSettings")
                .Get<ExchangeRateSourceSettings>();
        }

        public IEnumerable<ExchangeRate> GetExchangeRates()
        {
            CommonValidationTool.Validate(new ExchangeRateSourceSettingValidator(), _exchangeRateSourceSettings);

            var getExchangeRatesFromSourceAsString = getExchangeRatesFromSource().Result;
            var parsedExhangeRates = getExchangeRatesFromParser(getExchangeRatesFromSourceAsString);

           return parsedExhangeRates;   
        }

        private Task<string> getExchangeRatesFromSource()
        {
            using (var httpClient = new HttpClient())
            {
                var sendExchangeRequestUrl = _exchangeRateSourceSettings.SourceUrl;

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
                Select(exchangeRateLineSplitted => new ExchangeRate(new Currency(_exchangeRateSourceSettings.SourceCurrency), new Currency(exchangeRateLineSplitted[3]), decimal.Parse(exchangeRateLineSplitted[4]))).ToArray();
        }
    }
}
