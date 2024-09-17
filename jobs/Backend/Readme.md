# Mews backend developer task

I developed an implementation of the Exchange Rate Provider that takes a list Currency objects & provides an ExchangeRate object for each that specifies the FX rate of that currency, using CZK as a base.

I made this decision because the source data from Czech National Bank (CNB) was solely based on the CZK currency, and although FX rates of other currency-pairs could be worked out from this information, it was specified to not calculate FX rates in the spec.

I implemented an API call to fetch the data from the CNB, and then wrote a parser class to read this data into Currency & Exchange rate objects.

As the instructions were to implement something that I would write as if I needed to maintain it, I chose to build a cache of TargetCurrency -> ExchangeRate key-value pairs. This would allow for fast lookup when the system is live, without needing to fetch from source every time. I also used Quartz to schedule a cache refresh for 1:30pm every day*, as the Czech National Bank website stated that the FX rate data was updated once per day at 2:30pm, and we are 1 hour ahead in England. This would mean that the data is updated automatically when it is refreshed at the source.


I also added a unit test project, but didn't get to implement as many tests as I would usually like, so I opted to test areas where I could display my knowledge of the different types of unit test (Test & Testcase tests in NUnit, Fact & Theory tests in XUnit).

Given more time I would make the URL of the data source configurable in case this changes, and I would extend the cache class to account for FX rates for different base currency (this would simply involve turning the ExchangeRate value of the TargetCurrency/ExchangeRate key-value pair into a list of ExchangeRates and then querying the cache for both Base & Target currency when calling the GetExchangeRateAsync method).

 


