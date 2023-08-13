# Exchange rate provider for Czech National Bank implementation (.Net)

### Solution implemented according following Task description:
    
 The task is to implement an [ExchangeRateProvider](ExchangeRateProviderLib/ExchangeRateProvider.cs) for Czech National Bank. Find data source on their web
- part of the task is to find the source of the exchange rate data and a way how to extract it from there.
It is up to you to decide which technology (from .NET family) or package to use. Any code design changes/decisions to the provided skeleton are also completely up to you.
 The solution has to be buildable, runnable and the test program should output the obtained exchange rates.
Goal is to implement a fully functional provider based on real world public data source of the assigned bank.
To submit your solution, just open a new pull request to this repository.
Please write the code like you would if you needed this to run on production environment and had to take care of it long-term.

 Also following comment from "GetExchangeRates" method was taken into account as part of task description:
/// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
/// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
/// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
/// some of the currencies, ignore them.

### About exchange rates from CNB:
    
 CNB provides 2 groups of exchange rates 
1) Group that gets published every working day. [More info](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/)
2) Group that gets published every month. [More info](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/)

### Solution's building blocks:
    
 Current directury contains visual studio solution with application to get [exchange rates](https://www.investopedia.com/terms/e/exchangerate.asp) of Czech National Bank.
As a source of exchange rates used [rest API provided by the CNB](https://api.cnb.cz/)
The solution consist of Docker Compose project, client dll C# project and 'demo' C# console project.
Docker Compose project consists 2 docker containers one container with SQL server with data base and one 
container with .Net Service Worker.

### Assumptions:
 Solution is designed with following assupmtions in mind:
1)Client code that will use Exchage rates provider is on our side and not on a client side.
2)Exchange rate rpovider is expected to be scalable

### How solution works:
 Worker service Updates Exchange rates in the data base every time CNB publishes the updates.
Client side code requests rates from the Data Base. That provides scalability and makes whole application more robust -
We wont depend on CNB systems availability too much and number of calls to CNB systems will remain constant does not matter
how many clients we have and howe many times they make requests for exchange rates. Performance of the application can be achieved
by adding more sql server database instances.

#What I would improve as a next steps:
1) I would add interface to provide next years public holidays (In the solution information about public holidays is used to calculate validity of
the exchange rates)
2) I would double check and make sure that api.cnb.cz is really published by CNB and which limitations it has.
3) I would add .Net Web Api project to provider exchange rates to clients via rest api instead of direct data base access.
4) I would rewrite service worker using Domain Driven Develpment methodology.
5) I would Add integration and unit tests



