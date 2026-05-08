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

### Discovery
I navigated the CNB website to retrieve information around any API/data that relates to Exchange rates and found the following link describing an API.
https://www.cnb.cz/cs/casto-kladene-dotazy/Kurzy-devizoveho-trhu-na-www-strankach-CNB/

For this project I wanted to be able to easily extract the data regarding exchange rates, there were two approaches to this:
- Option 1: Take advantage of the Swagger docs and directly integrate with the API: https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates 
- Option 2: Utilising the TXT file directly from the page https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/

For a production grade system, I decided to take advantage of the Swagger Docs as they are a typical gold standard for developers on information regarding what can be returned/errors,
and it is safe to assume the route won't change for the API whereas the website can change.

### Programming Philosophies That I followed for this project 
- Adherence to SOLID principals
- Testability
- Readability
- Observability
- KISS & DRY

### Project Configuration
With my approach I decided to upgrade the project to utilise .net 10.
It required little effort and would provide a long term benefit in terms of active support in a production environment and ensure security updates are available.
I utilised the option pattern so that API configuration can be done with a simple application.json modification and allow for a source currency to be defined.

Additionally I added Polly and FluentValidation to assist with building a more resilient system to solving transient network errors and validating user input respectively.
The Test project mimics the folder structure of the ExchangeRateUpdater Project so that it is easy to identify where the tests are. They were also written with BDD in mind.

I decided to define a contract `IExchangeRateClient` where if we needed to integrate with another API, we could be free to do so without much hassle

## Core Principles for the development in the project
## Resiliency 
This was achieved using Polly in the CzechNationalBankExchangeRateClient. 
This client uses retrys and timeouts to manage the duration of a request and give it the ablilty to deal with transient errors.
These values are configured in appSettings.json

## Observability
 - When it comes to observability, I try to follow the idea of being able to "Follow your code via Logs", and this was a practice that I tried to follow this project too.
I not only accounted for error cases but also what should happen normally e.g, if we recieve a response from the external api. 
Additionally i've accounted for exceptions and Logged them individually to identify specific errors.
I've also created a `ErrorLog` to extract the important information like the status code and message should there be an error.

## Maintainability
- I've explicitly created a "EmptyResponse" object to handle the cases where there is an unexpected failure. 
This avoids returning a "null" that is just obscure but instead returns a specific object that describes a fault.
- I've also used the ResultsPattern to help identify successful outcomes to those that are undesired.
- Single responsiblity has been a core component of this project as each class has one purpose in mind. It has allowed for simple and easy tests that are quick to verify.

### Testing
- FluentAssertions as they are really readable and assertions just make sense.
- Moq for mocking out dependencies; really useful and very familiar with/its wide known.
- XUnit as familiar framework.

### Testing Strategy
- Generally I try to stay close to the Given_When_Then format as its clear to identify the objective behind a test.
- I prioritised mocking dependencies and Followed the Arrange Act Assert Pattern to make the tests clear to a reader.
- Utilising SOLID patterns also helped make tests easier as the objective behind a class was straight forward.
- Tests share an identical name with their class and follow a similar folder structure to ensure easy traceability.

What this project does:
- Exposes a Swagger Endpoint for interacting with the API 
  - `api/v1/exchange-rates/daily`
    - This endpoint fetches the desired currencies and returns the Exchange based on the Source Currency for the current business day.

## Getting Started

Follow these steps to build, run, and test the project

### 1. Build the Solution
Compile the entire solution to resolve dependencies and create binaries.
```bash
dotnet build ExchangeRateUpdater.sln
```

### 2. Run the Solution
Compile the entire solution to resolve dependencies and create binaries.
```bash
dotnet run --project ExchangeRateUpdater/ExchangeRateUpdater.csproj
```

### 3. Call the endpoint
Utilise the Swagger Docs (Accessible via /swagger/index.html)

Alternatively you can use the following CURL or Call the endpoint `api/v1/exchange-rates/daily` via Postman/Insomnia.

```markdown
curl -X 'GET' \
  'http://localhost:5001/api/v1/exchange-rates/daily?currencies=USD&currencies=GBP&currencies=XAK' \
  -H 'accept: application/json'
```

### Run the Tests
```bash
dotnet test ExchangeRateUpdater.Tests/ExchangeRateUpdater.Tests.csproj
```

### Note
When running locally, utlilse the LaunchSettings configuration so that its simpler to run.

Things I would do if I had more time:
- Support for the ability to choose a specific date, this could be done via utlising the existing integrated API or Expanding to using the other Endpoints.
  - This can easily be done by taking advantage of the `IExchangeRateClient` and `CzechNationalBankExchangeRateClient` since there is a defined contract to retrieve ExchangeRate Data and the Client is open to expansion.
- Caching the API response to reduce unnecessary requests calls to the CNB api.
- Add robust integration tests utilising the JustEat client for Interception https://github.com/justeattakeaway/httpclient-interception 
- A suite of automated Postman tests used as a smoke test between environments to ensure confidence that the system still works.
- Setup Runbook Alerts within monitoring Frameworks like Datadog so that any unexpected errors are raised to the attention of a developer asap.
