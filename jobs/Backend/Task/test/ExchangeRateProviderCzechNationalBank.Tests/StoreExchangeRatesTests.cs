using ExchangeRateProvider.Contracts;
using ExchangeRateProviderCzechNationalBank.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRateProviderCzechNationalBank.Tests
{
    public class StoreExchangeRatesTests
    {
        private readonly StoreExchangeRates _sut;

        public StoreExchangeRatesTests()
        {
            _sut = new StoreExchangeRates();
        }


        public static readonly object[][] UpdateRates_Ignores_NullAndEmptyListData =
        {
            new object[]{null},
            new object[]{new List<ExchangeRate>()}
        };
        [Theory, MemberData(nameof(UpdateRates_Ignores_NullAndEmptyListData))]
        public void UpdateRates_Ignores_NullAndEmptyList(List<ExchangeRate> input)
        {
            var now = new DateTime(2023, 1, 10, 10, 10, 10);

            _sut.UpdateRates(input, now);

            Assert.Equal(DateTime.MinValue, _sut.RatesUpdatedOn);
            Assert.Equal(input != null ? input.Count : 0, _sut.TestGetRates().Count);
        }

        [Fact]
        public void UpdateRates_Updates_RatesUpdatedOn()
        {
            var now = new DateTime(2023, 1, 10, 10, 10, 10);

            _sut.UpdateRates(new List<ExchangeRate> 
                { 
                    new ExchangeRate(new Currency("AAA"), new Currency("BBB"), 10)
                }, now);

            var a = _sut.RatesUpdatedOn;

            Assert.Equal(now, _sut.RatesUpdatedOn);
        }

        [Fact]
        public void UpdateRatesWithoutTime_DoesNotUpdates_RatesUpdatedOn()
        {
            _sut.UpdateRates(new List<ExchangeRate>());

            Assert.Equal(DateTime.MinValue, _sut.RatesUpdatedOn);
        }

        public static readonly object[][] GetRates_Returns_ExpectedRatesData =
        {
            new object[]{new DateTime(2023, 1, 10, 10, 10, 10),
                new List<ExchangeRate>()
                {
                    new ExchangeRate(new Currency("AAA"), new Currency("BBB"), 10),
                    new ExchangeRate(new Currency("CCC"), new Currency("DDD"), 20)
                }, new List<Currency>() {new Currency("AAA")}, 10 },
            new object[]{new DateTime(2023, 1, 10, 10, 10, 10),
                new List<ExchangeRate>()
                {
                    new ExchangeRate(new Currency("AAA"), new Currency("BBB"), 10),
                    new ExchangeRate(new Currency("CCC"), new Currency("DDD"), 20)
                }, new List<Currency>() {new Currency("CCC")}, 20 }
        };
        [Theory, MemberData(nameof(GetRates_Returns_ExpectedRatesData))]
        public void GetRates_Returns_ExpectedRates(DateTime now, List<ExchangeRate> rates, List<Currency> lookedForCurrencies, decimal expectedRate)
        {
            _sut.UpdateRates(rates, now);

            var result = _sut.GetRates(lookedForCurrencies);

            Assert.Equal(now, _sut.RatesUpdatedOn);
            Assert.Equal(lookedForCurrencies.First().Code, result.First().SourceCurrency.Code);
            Assert.Equal(expectedRate, result.First().Value);
        }

        public static readonly object[][] GetRates_SearchesBy_TargetCurrencyData =
        {
            new object[]{new DateTime(2023, 1, 10, 10, 10, 10),
                new List<ExchangeRate>()
                {
                    new ExchangeRate(new Currency("AAA"), new Currency("BBB"), 10),
                    new ExchangeRate(new Currency("CCC"), new Currency("DDD"), 20)
                }, new List<Currency>() {new Currency("AAA"), new Currency("DDD")} }
        };
        [Theory, MemberData(nameof(GetRates_SearchesBy_TargetCurrencyData))]
        public void GetRates_SearchesBy_TargetCurrency(DateTime now, List<ExchangeRate> rates, List<Currency> lookedForCurrencies)
        {
            _sut.UpdateRates(rates, now);

            var result = _sut.GetRates(lookedForCurrencies);

            Assert.Equal(now, _sut.RatesUpdatedOn);
            Assert.Single(result);
            Assert.Equal(lookedForCurrencies.First().Code, result.First().SourceCurrency.Code);
        }
    }
}
