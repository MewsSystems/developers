using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    ///<inheritDoc/>
    internal class CnbApiClient : ICnbApiClient
    {
        private Uri _DailyExchangeRatesUri = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt");
        private Uri _MonthlyExchangeRatesUri = new Uri("https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt");

        private readonly IExchangeRatesListingParser _Parser;
        public CnbApiClient(IExchangeRatesListingParser parser) {
            _Parser = parser;
        }

        public async Task<ExchangeRatesListing> GetDailyExchangeRates() {
            string dailyExchangeRatesString;
            try {
                using (var client = new HttpClient()) {
                    dailyExchangeRatesString = await client.GetStringAsync(_DailyExchangeRatesUri);
                }
            }
            catch (Exception ex){
                throw new HttpRequestException($"Error occured while trying to get daily exchange rates information from the CNB website. {ex}");
            }

            return _Parser.ParseExchangeRatesListingString(dailyExchangeRatesString);
        }

        public async Task<ExchangeRatesListing> GetMonthlyExchangeRates() {
            string monthlyExchangeRatesString;
            try {
                using (var client = new HttpClient()) {
                    monthlyExchangeRatesString = await client.GetStringAsync(_MonthlyExchangeRatesUri);
                }
            }
            catch (Exception ex) {
                throw new HttpRequestException($"Error occured while trying to get mothly exchange rates information from the CNB website. {ex}");
            }

            return _Parser.ParseExchangeRatesListingString(monthlyExchangeRatesString);
        }
    }
}
