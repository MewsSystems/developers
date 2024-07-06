# Mews backend developer task (.NET)

## Requirement
The task is to implement an [ExchangeRateProvider](Task/ExchangeRateProvider.cs) for Czech National Bank. Find data source on their web - part of the task is to find the source of the exchange rate data and a way how to extract it from there.

It is up to you to decide which technology (from .NET family) or package to use. Any code design changes/decisions to the provided skeleton are also completely up to you.

The solution has to be buildable, runnable and the test program should output the obtained exchange rates.

Goal is to implement a fully functional provider based on real world public data source of the assigned bank.

To submit your solution, just open a new pull request to this repository.

Please write the code like you would if you needed this to run on production environment and had to take care of it long-term.

## Solution (Akintunde Olanrewaju)
### Understanding of the requirement
My understanding of the requirement is to build a .Net application that handles fetching of exchange rate for a base currency from an exchange rate provider against a well defined set of target currencies. Where any of the target currency is not found, the target currency is skipped in the response.

### Overview of the solution provided
I have used Net8.0 to build an MVC application that exposes an API to fetch daily exchange rate of a base currency currency against one or more target currency/currencies. The API allows for the consumer to specify the target date and language of choice.
The application has the flexibility of allowing consumers to specify the base currency (CZK, this context), thereby making the application extensible. However, to support other base currencies asides CZK, an integration has to be done with the exchange provider and appropriate application configuration values are set for the integrated provider.
Also, this application improves application performance by maintaining a cache (In-memory storage used in this solution) with a smart cache eviction strategy. This makes the application faster as exchange rate sources (based on my investigation) release exchange rate once every working day.
In order to make the application more dynamic, it asynchronously pulls data from the provider at the communicated exchange rate refresh time (2:30PM Czech local time for the Czech National Bank). This is achieved by installing a cronjob/worker that, at the background, pulls data from the provider and refreshes the cache. This cronjob installation was motivated by the project name "exchange rate 'updater'"

### How to test the application
1. Pull the solution branch with name **"akintunde-olanrewaju-exchange-rate-updater"**
2. Just like any other .Net application, run the applicaton. The application exposes itself on https://localhost:50176 and http://localhost:50177
3. Hit its single API using the Postman on https://localhost:50176/api/v1/exchange-rate/daily-rate?date=2020-06-06&language=EN&baseCurrency=CZK&targetCurrencies=USD,EUR,TRY

NB: I did a few scenario testing on Postman, please find the saved responses from the application [here](https://warped-astronaut-42587.postman.co/workspace/My-Workspace~724b6986-7c71-49e7-87ae-018eaece97a0/folder/3890379-a41ded17-ea5c-4016-8c12-55ad6d833e24?action=share&creator=3890379&ctx=documentation)


### Appendices

##### Postman
![x2](https://github.com/SirGoingFar/developers/assets/35031739/97a3ecb9-a947-4946-bb60-556094f0621b)

##### Cronjob/Worker log
![x1](https://github.com/SirGoingFar/developers/assets/35031739/d3eb00c3-636c-41df-a94e-af8c6d22da2e)
