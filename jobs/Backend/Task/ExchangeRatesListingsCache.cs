using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRateUpdater
{
    ///<inheritDoc/>
    public class ExchangeRatesListingsCache : IExchangeRatesListingsCache
    {
        private readonly IDateTimeProvider _DateTimeProvider;
        private readonly IBankDateProvider _BankDateProvider;
        private readonly ICnbApiClient _CnbApiClient;

        private ExchangeRatesListing _LastMonthlyExchangeRatesListing;
        private ExchangeRatesListing _LastDailyExchangeRatesListing;
        private DateTime? _LastMonthlyListingCallTimestamp;
        private DateTime? _LastDailyListingCallTimestamp;

        private static readonly TimeSpan _MinimumTimeBetweenClientCalls = TimeSpan.FromMinutes(5);

        public ExchangeRatesListingsCache(IDateTimeProvider dateTimeProvider, IBankDateProvider bankDateProvider, ICnbApiClient cnbApiClient) {
            _DateTimeProvider = dateTimeProvider;
            _BankDateProvider = bankDateProvider;
            _CnbApiClient = cnbApiClient;
        }

        ///<inheritDoc/>
        public async Task<IReadOnlyList<ExchangeRate>> GetCurrentExchangeRates() {
            if (ShouldRefreshDailyListing()) {
                await RefreshDailyListing();
            }
            if (ShouldRefreshMonthlyListing()) {
                await RefreshMonthlyListing();
            }

            return GetAllAvailableExchangeRates();
        }

        private IReadOnlyList<ExchangeRate> GetAllAvailableExchangeRates() {
            return _LastDailyExchangeRatesListing.ExchangeRates.Concat(_LastMonthlyExchangeRatesListing.ExchangeRates).ToList();
        }

        private async Task RefreshDailyListing() {
            var newDailyListing = await _CnbApiClient.GetDailyExchangeRates();
            _LastDailyListingCallTimestamp = _DateTimeProvider.Now();
            _LastDailyExchangeRatesListing = newDailyListing;
        }

        private async Task RefreshMonthlyListing() {
            var newMonthlyListing = await _CnbApiClient.GetMonthlyExchangeRates();
            _LastMonthlyListingCallTimestamp = _DateTimeProvider.Now();
            _LastMonthlyExchangeRatesListing = newMonthlyListing;
        }

        private bool ShouldRefreshDailyListing() {
            if (_LastDailyExchangeRatesListing == null || _LastDailyListingCallTimestamp == null) {
                return true;
            }

            var currentBankDay = _BankDateProvider.GetDailyListingBankDateForDateTime(_DateTimeProvider.Now());
            if (currentBankDay <= _LastDailyExchangeRatesListing.ListingDate) {
                return false;
            }

            if ((_DateTimeProvider.Now() - _LastDailyListingCallTimestamp) < _MinimumTimeBetweenClientCalls) {
                return false;
            }

            return true;
        }

        private bool ShouldRefreshMonthlyListing() {
            if (_LastMonthlyExchangeRatesListing == null || _LastMonthlyListingCallTimestamp == null) {
                return true;
            }

            var currentBankMonth = _BankDateProvider.GetMonthlyListingBankDateForDateTime(_DateTimeProvider.Now());
            if (currentBankMonth <= _LastMonthlyExchangeRatesListing.ListingDate) {
                return false;
            }

            if ((_DateTimeProvider.Now() - _LastMonthlyListingCallTimestamp) < _MinimumTimeBetweenClientCalls) {
                return false;
            }

            return true;
        }
    }
}
