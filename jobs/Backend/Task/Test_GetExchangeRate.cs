using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace ExchangeRateUpdater
{
    public class Test_GetExchangeRate
    {
        static public string _GetTestData()
        {
            string original =  @"08 Jan 2024 #5
                    Country|Currency|Amount|Code|Rate
                    Australia|dollar|1|AUD|14.951
                    Brazil|real|1|BRL|4.570
                    Bulgaria|lev|1|BGN|12.517
                    Canada|dollar|1|CAD|16.712
                    China|renminbi|1|CNY|3.123
                    Denmark|krone|1|DKK|3.281
                    EMU|euro|1|EUR|24.480
                    Hongkong|dollar|1|HKD|2.865
                    Hungary|forint|100|HUF|6.484
                    Iceland|krona|100|ISK|16.244
                    IMF|SDR|1|XDR|29.848
                    India|rupee|100|INR|26.910
                    Indonesia|rupiah|1000|IDR|1.441
                    Israel|new shekel|1|ILS|6.021
                    Japan|yen|100|JPY|15.476
                    Malaysia|ringgit|1|MYR|4.809
                    Mexico|peso|1|MXN|1.324
                    New Zealand|dollar|1|NZD|13.905
                    Norway|krone|1|NOK|2.155
                    Philippines|peso|100|PHP|40.191
                    Poland|zloty|1|PLN|5.633
                    Romania|leu|1|RON|4.923
                    Singapore|dollar|1|SGD|16.802
                    South Africa|rand|1|ZAR|1.194
                    South Korea|won|100|KRW|1.694
                    Sweden|krona|1|SEK|2.184
                    Switzerland|franc|1|CHF|26.310
                    Thailand|baht|100|THB|63.842
                    Turkey|lira|100|TRY|74.818
                    United Kingdom|pound|1|GBP|28.423
                    USA|dollar|1|USD|22.366";

            string[] lines = original.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            return string.Join('\n', lines);
        }


        static public string GetTestData_No_Header()
        { 
            string data = _GetTestData();

            string[] lines = data.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            
            List<string> noHeader =  lines.Except(new string[] { "Country|Currency|Amount|Code|Rate" }).ToList();

            return string.Join('\n', noHeader.ToArray());
        }

        static public string GetTestData_No_Rates_For_Australia()
        {
            string data = _GetTestData();

            string[] lines = data.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            lines[2] = "Australia|dollar|1|AUD|"; // remove the rate

            return string.Join('\n', lines);
        }


        // because the header is missing no data will be parsed
        // we start parsing from the header downwards
        [Fact]
        public void Test_ParseBankResponse()
        {
            string data = GetTestData_No_Header();
            GetExchangeRate getExchangeRate = new GetExchangeRate(data);
            IEnumerable<ExchangeRate> rates = getExchangeRate.ParseBankResponse();

            Assert.Empty(rates);
        }

        [Fact]
        public void Test_ParseDataRow()
        {
            string data = GetTestData_No_Header();
            GetExchangeRate getExchangeRate = new GetExchangeRate(data);
            IEnumerable<ExchangeRate> rates = getExchangeRate.ParseBankResponse();

            ExchangeRate rate = getExchangeRate.ParseDataRow("Australia|dollar|1|AUD|14.951");

            Assert.NotNull(rate);
            Assert.Equal("CZK", rate.TargetCurrency.Code);
            Assert.Equal("AUD", rate.SourceCurrency.Code);
            Assert.Equal((decimal)14.951, rate.Value);
        }
        
        [Fact]
        public void Test_ParseDataRow_Invalid_Columns()
        {
            string data = GetTestData_No_Rates_For_Australia();
            GetExchangeRate getExchangeRate = new GetExchangeRate(data);
            
            IEnumerable<ExchangeRate> rates = getExchangeRate.ParseBankResponse();

            ExchangeRate australiaRate = rates.SingleOrDefault(r => r.SourceCurrency.Code == "AUD");

            Assert.Null(australiaRate);
        }

        [Fact]
        public void Test_ParseDataRow_Invalid_Rate()
        {
            string data = _GetTestData();
            GetExchangeRate getExchangeRate = new GetExchangeRate(data);

            IEnumerable<ExchangeRate> rates = getExchangeRate.ParseBankResponse();

            ExchangeRate rate = getExchangeRate.ParseDataRow("Australia|dollar|1|AUD|");

            Assert.Null(rate);
        }
    }
}
