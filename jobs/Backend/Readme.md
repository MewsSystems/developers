# Exchange rate solution

Gets exchange rate data from Czech National Bank.

## Prerequisites

[.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)

## Data sources

- [TXT file - CNB website](https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt)
- [XML file - CNB website](https://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.xml)

## Solution parts

The project consists of these parts:

- **Framework** - Divided into multiple project so that we can use it later for nuget creation and artifactory.

  - **Framework.BaseHttpClient** - Base http client with basic status code handling
  - **Framework.Caching** - In-memory cache implementation, we can switch in-memory cache to redis or different one
  - **Framework.Converters** - XML converter, prepared for other common converters
  - **Framework.Exceptions** - Exception models
  - **Framework.Logging** - Setting up logging configuration, prepared for logging
  - **Framework.Unittests** - Framework unit tests

- **Service** - hold the logic and data specific to a client that provides the exchange rate
  - **Clients** - prepared for different clients, not only CNB
    - **ExchangeRate.Client.Cnb** - CNB client that provide the exchange rate from various sources
  - **ExchangeRate.Domain** - Contains domain objects (Currency, Ehange rate)
  - **ExchangeRate.Service** - Contains logic which gets data from Clients like CNB client
  - **ExchangeRate.UnitTests** - Exchange rate solution unit tests
- **Solution Items** - Contain .editorconfig for multi developer/IDE development that everybody will use defined style
- **ExchangeRate.Console** - .net console app
- **ExchangeRate.WebApi** - .net Web API for fetching exchange rates

## Project overview

### System architecture

The architecture diagram to be used within draw.io can be found in `/jobs/Backend/doc/Architecture.drawio`
![Architecture](/jobs/Backend/doc/img/SystemArchitecture.png)

### Solution architecture

![Architecture](/jobs/Backend/doc/img/SolutionArchitecture.png)

- Caching - Exchange rate is using in-memory caching. Later we can use multiple instancies and we can change in-memory cache to redis cache or different one.
- Logging - Using standard `Microsoft.Extensions.Logging` using ILogger interface. Configuration is stored in appsettings.json key "Logging"

```json
"Logging": {
		"LogLevel": {
			"Default": "Information",
			"Microsoft.AspNetCore": "Warning",
			"System.Net.Http.HttpClient": "Warning"
		}
	}
```

- Retry strategy - Http client is using retry strategy via `Polly` extension. Retry count is defined in appsettings.json key: CnbClientConfiguration.Retry

```json
"CnbClientConfiguration": {

		"Retry": 3
	}
```

- Testing strategy - Framework and Exchange rate are covered by unit testing (38 unit tests). Using Xunit framework with Moq.
  ![Unit testing](/jobs/Backend/doc/img/UnitTestsFullSolution.jpg)
- Web api features - values: true or false
  - EnableCorrelationIdMiddleware - enable or disable correlation id middleware
  - EnableExceptionHandlingMiddleware - enable or disable global exception handling middleware
  - EnableGetCnbExchangeRates - enable or disable GetCnbExchangeRates in ExchangeRate Controller

## How to run exchange rate application

### Console app

```
// Run exchange rates without parameter => this will use default Xml implementation
dotnet ExchangeRate.Console.dll
// Run exchange rates for txt source
dotnet ExchangeRate.Console.dll CnbTxt
// Run exchange rates for XML source
dotnet ExchangeRate.Console.dll CnbXml
```

Console result example

```
**********
* Result *
**********
EUR/CZK=24,660
JPY/CZK=0,18225
THB/CZK=0,67971
TRY/CZK=1,468
USD/CZK=23,320
```

### WebApi

Using swagger.

```
https://<url>/swagger/index.html
```

Call Get method directly

```
// Run exchange rates without parameter => this will use default Xml implementation where apiType=CnbXml
https://<url>/ExchangeRate

// Run exchange rates for txt source where apiType=CnbTxt
https://<url>/ExchangeRate?apiType=CnbTxt

// Run exchange rates for XML source where apiType=CnbXml
https://<url>/ExchangeRate?apiType=CnbXml
```

Api result example

```
["EUR/CZK=24,660","JPY/CZK=0,18225","THB/CZK=0,67971","TRY/CZK=1,468","USD/CZK=23,320"]
```
