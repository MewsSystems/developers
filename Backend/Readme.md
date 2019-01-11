# Mews backend developer task

The task is to implement an [ExchangeRateProvider](Task/ExchangeRateProvider.cs) for Czech National Bank. Find data source on their web. It is up to you to decide which technology (from .NET FW) or package to use. Part of the task is to find the source of the exchange rate data and a way how to extract it from there.

The solution has to be buildable, runnable and the test program should output the exchange rates obtained at the run time. Please submit the solution in form of a pull request to this repository.

This is to implement a fully functional provider based on real world public data source of the assigned bank. For some of the banks, the fastest applicants managed to implement this in less than an hour.

To sumbit your solution, just open a new pull request to this repository.

## Additional Information

As the data source from CNB I am using the daily report made available as a text file at the following URL: http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt

By default when the application is run, it will report only exchange rates between the specified currency codes and CZK, as this is the baseline currency used by CNB. Out of interest I have provided
optional functionality to infer the exchange rates to retrieve a richer data set as I was interested to see how I could implement it. This functionality is covered by a unit test, or can be invoked by  
setting the the optional boolean parameter getCalculatedRates in the call to RateAdapter.GetExchangeRateData in the ExchangeRateProvider class.