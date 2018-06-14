using System;
using ExchangeRateUpdater;

namespace ExchangeRateUpdaterTests.Mocks
{
    public class TestWebClientWrapper : IWebClientWrapper
    {
        public string DownloadString(Uri address)
        {
            return @"11.Jun 2018 #111
Country|Currency|Amount|Code|Rate
Australia|dollar|1|AUD|16.566
Brazil|real|1|BRL|5.851
Bulgaria|lev|1|BGN|13.129
Canada|dollar|1|CAD|16.735
China|renminbi|1|CNY|3.402
Croatia|kuna|1|HRK|3.480
Denmark|krone|1|DKK|3.447
EMU|euro|1|EUR|25.680
Hongkong|dollar|1|HKD|2.776
Hungary|forint|100|HUF|7.984
Iceland|krona|100|ISK|20.560
IMF|SDR|1|XDR|30.913
India|rupee|100|INR|32.305
Indonesia|rupiah|1000|IDR|1.564
Israel|shekel|1|ILS|6.097
Japan|yen|100|JPY|19.813
Malaysia|ringgit|1|MYR|5.462
Mexico|peso|1|MXN|1.067
New Zealand|dollar|1|NZD|15.319
Norway|krone|1|NOK|2.703
Philippines|peso|100|PHP|41.049
Poland|zloty|1|PLN|6.018
Romania|new leu|1|RON|5.512
Russia|rouble|100|RUB|34.798
Singapore|dollar|1|SGD|16.317
South Africa|rand|1|ZAR|1.657
South Korea|won|100|KRW|2.025
Sweden|krona|1|SEK|2.504
Switzerland|franc|1|CHF|22.081
Thailand|baht|100|THB|67.902
Turkey|lira|1|TRY|4.819
United Kingdom|pound|1|GBP|29.121
USA|dollar|1|USD|21.781";
        }
    }
}