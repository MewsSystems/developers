### Thoughts

## Building a requirements List

The task is to implement an ExchangeRateProvider for Czech National Bank. 
Find data source on their web - part of the task is to find the source of the exchange rate data and a way how to extract it from there.
It is up to you to decide which technology (from .NET family) or package to use. 
Any code design changes/decisions to the provided skeleton are also completely up to you.
The solution has to be buildable, runnable and the test program should output the obtained exchange rates.
Goal is to implement a fully functional provider based on real world public data source of the assigned bank.
To submit your solution, create a Pull Request from a fork.

Please write the code like you would if you needed this to run on production environment and had to take care of it long-term.

Should return exchange rates among the specified currencies that are defined by the source. But only those defined
by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
some of the currencies, ignore them.

Translates to:
    - Finding a datasource
    - Able to state what Exchange rates we want to return based on a list of desired currencies
    - A "source" currency to calculate against.
    - A Desired response to return

https://www.cnb.cz/cs/casto-kladene-dotazy/Kurzy-devizoveho-trhu-na-www-strankach-CNB/

For this project I wanted to be able to easily extract the data regarding exchange rates, there were two approaches to this
 - Option 1: Take advantage of the Swagger docs and directly integrate with the API:  https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates 
 - Option 2: Utilising the TXT file directly from the page https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/

For a production grade system, I decided to take advantage of the Swagger Docs as they are a typical gold standard for developers on information regarding what can be returned/errors,
and it is safe to assume the route won't change for the API whereas the website can change.

What this project does:

- Exposes a Swagger Endpoint for interacting with the API 
  - ``api/v1/exchange-rates/daily``


## Getting Started

Follow these steps to build, run, and test the project

### 1. Build the Solution
Compile the entire solution to resolve dependencies and create binaries.
```bash
dotnet build ExchangeRateUpdater.sln
```

```bash
dotnet run --project ExchangeRateUpdater/ExchangeRateUpdater.csproj
```

```bash
dotnet test ExchangeRateUpdater.Tests/ExchangeRateUpdater.Tests.csproj
```

Things I would do if I had more time:
    - Support for the ability to choose a specific date.
    - Caching the API response to reduce unnecessary requests calls to the CNB api.
    - Add robust integration tests utilising the JustEat client for Interception https://github.com/justeattakeaway/httpclient-interception 
    - A suite of automated Postman tests used as a smoke test between environments to ensure confidence that the system still works.
    - Setup Runbook Alerts within monitoring Frameworks like Datadog so that any unexpected errors are raised to the attention of a developer asap.



