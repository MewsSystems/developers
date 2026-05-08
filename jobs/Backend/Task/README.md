# ExchangeRateUpdater Task Solution Explanation

## Research of data source

To implement the task, I began browsing the Czech National Bank's website to find where to obtain current exchange rates. I discovered that from [this page](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/) they could be retrieved, as it shows the current exchange rates for all the available currencies from the bank. Also, you could retrieve the data in text format by clicking the "Text format" button at the bottom, making it easier to retrieve the values.

However, the GetExchangeRates method that must be implemented, states that "should return exchange rates among the specified currencies that are defined by the source. But only those defined by the source". Therefore, if the exchange rates were obtained from this data source, for every currency from the bank would have to iterate over the specified currencies and check if it appears in them, which would slow down the method execution if many specified currencies were unavailable.

Nevertheless, in [this other page](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/selected_form.html) the exchange rates can be obtained for a specific month and for a selected currency, which might be more convenient and faster, as we could iterate over the specified currencies and check whether there is or not available data. The output format can be chosen between "Html", "Text" and "Excel", of which I chose the "Text" format because it makes it easier to extract the exchange value.

If we set for example March 2025 for EUR and "Text" format, we get [this output](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/selected.txt?from=01.03.2025&to=31.03.2025&currency=EUR&format=txt). As we can see, it shows all the EUR exchange rates throughout the month. If we look at the URL that retrieves this data source, we can see that it uses four query parameters in the API request: "from", "to", "currency" and "format". Knowing this, we could change their values to get different exchange rates for the selected time range and currency. For this reason, I chose this Uri from the Czech National Bank to retrieve the exchange rates.

## ExchangeRateProvider implementation

As we can deduce by the output the request shows, the exchange rate values always have the specified currency as source currency and CZK as target currency. Also, the output mentions an amount, meaning that the exchange rates are calculated as: Amount\*1 SourceCurrency = X CZK, where X is the displayed exchange rate. Because of this, since the ExchangeRate class from the skeleton has no amount value, I decided to make the conversion so the exchange rates would always show as: 1 SourceCurrency = X CZK.

If we try to input in the "currency" query parameter a code that is not available in the bank, we can see that the API request succeeds, but the output is empty and the content length is 0, so we can know that there are no exchange rates available for the specified currency if this happens.

However, as stated on [this page](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/), the exchange rates are declared on working days, so if we set only a time period of today and it is a holiday, we could not retrieve the current exchange rate. In order to avoid that, I chose to do the API request for each currency within a month from today, so in that way we can make sure that if there is no data, the currency is not available in the bank.

From the output we can see that the amount could be retrieved from the first line, and the current exchange rate from the last line; so I retrieved the corresponded values from the output by knowing the string index of each line where the value starts.

Finally, with the retrieved exchange rates from the selected currencies, a list of ExchangeRate is returned. The rest of the task skeleton has remained the same, except for the ExchangeRateProvider constructor, where I added that an HttpClient object must be specified during object initialization, so by this I could make mocks from the client and make unit tests for the GetExchangeRates method.

## Production environment practices

I tried to write the code like I would if it would be run in production. Some good practices that I followed are:

- Make clean code by naming variables with self explanatory names and following the naming conventions for C#.

- Handle possible exceptions in the GetExchangeRates method that I know that could be raised: HttpRequestException if there is any error while doing the API request to get the exchange rates of some currency; and FormatException, as I retrieve the amount and rate from the output by knowing the string index where they start. Therefore, if the bank changes the output format at any time, this exception will be raised.

- Make unit tests for the implemented function, so it could be easily checked that everything goes well before uploading to production. To implement the tests I used the xUnit testing tool, wrote self explanatory test names following the schema MethodName_Scenario_ExpectedBehavior, and implemented the test cases with the recommended skeleton of AAA: Arrange, Act and Assert.
