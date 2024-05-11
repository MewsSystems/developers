# Exchange Rate Updater App
A small demo app for grabbing exchange rate information from Czech National Bank (CNB), mainly based around the `ExchangeRateProvider` in `ExchangeRateUpdater.Infrastructure.ExternalAPIs`

## Usage
The `ExchangeRateUpdater.Presentation.Cli` application accepts command line arguments in the standard format.
### Available Args 

- -c | --currencies
  - Required
  - A comma-separated list of currency codes to search against
- -d | --date
  - Optional
  - A date in the format yyyy-MM-dd to get exchange rates for a specific date
- --help
  - Prints the help text for this application
- --version
  - Prints the version of this application

### Docker
To run the app via docker, first ensure you have built an image from the Dockerfile in `ExchangeRateUpdater.Presentation.Cli` (and have docker desktop running).

Then you can use `docker run` to run the image, passing command line arguments as normal.

e.g. 
- `docker run exchangerateupdater.presentation.cli --help`
- `docker run exchangerateupdater.presentation.cli -c "USD, GBP, JPY"`
- `docker run exchangerateupdater.presentation.cli -d "2024-01-01" -c "USD, GBP, JPY"`

## Project Structure.
The project is structured following Domain-Driven Design.

### Application
Contains the use cases following CQRS pattern. Currently, there are only two: Get Exchange Rates and Get Exchange Rates for a specific Date.

This layer also contains the `ExchangeRateUpdater.Infrastructure.Interface` project in order to provide interfaces it can use to implement the use cases. The interface implementation is contained within the infrastructure layer.
### Domain
This contains the domain subject models used by other parts of the application.

### Infrastructure
This layer contains the implementation of the `ExchangeRateUpdater.Infrastructure.Interface` interfaces.
#### Extensions
The `ServiceDependenciesExtensions` contains extension methods used by any upstream project to register this implementation's dependencies into a Dependency Injection Container.
#### Models
The models contained here are specific to this implementation, they are therefore internal only records and should not be exposed to other projects (other than tests).
#### Clients
This directory contains implementation of the HTTP client used to access the CNB API.
#### Providers
This directory contains two implementations of the `IExchangeRateProvider`.

The first is a simple naive implementation that simply handles the calls to the client and converting the results to their domain representations.

The second is an implementation that uses caching to reduce the load on the external API as much as possible. See the Caching section below for more details.

## Caching
### Caching Philosophy
The results of the call to the CNB API all contain a `ValidFor` field that is a Date representing the date that the exchange rate is applicable to.

As the CNB rates are updated at 14:30 CEST daily we can use this `ValidFor` field to properly cache the daily rates for the correct date regardless of the timezone the app is running in. This completely avoids having to do any complex timezone & daylight savings maths to check when a cache entry should be valid until.

Caching the results of any HTTP call to the API for the returned `ValidFor` date means that any subsequent request for that date's exchange rates does not need to contact the API at all.

The cache functionality is configured via app configuration, it can be enabled or disabled and the expiration times can be customised.
For the purposes of this demo application the caching functionality does not provide much benefit as we are running in a command-line environment, a new process is started for each usage of the app.
However, if we were to implement a more realistic web based application to serve a long-running API, then this caching can be enabled and greatly improve both our application performance and also reduce the load on the external API.

### Potential Improvements
There are some improvements that could be made to improve the cache utilisation, dependent on further requirement clarification.

If a request is made to get exchange rates without specifying a date, then we default to the current UTC date. However, if it is currently before 14:30 CEST then the returned `ValidFor` date will be from the previous date (the latest exchange rates date the API has available).
This means that we will cache the response with a date of 2024-05-09 for example, when it is currently 2024-05-10 11:00 UTC, therefore any subsequent requests that do not specify the date will attempt to get the data for 2024-05-10 from the cache and find nothing; making another request to the API that is technically not required.

## Testing
The Unit Tests for this application are written using xUnit and should be runnable with standard `dotnet test` commands.
### Testing Philosophy
If you check the coverage for the unit tests you may see some exceptions, we don't bother explicitly excluding them from the test coverage because it's not worth it just to get a coverage number to go up.
Instead, this project opts to only test things that need testing, rather than testing for the sake of 100% coverage.

Therefore, things like the dependency injection extensions in `ExchangeRateUpdater.Infrastructure.ExternalAPIs` are not tested with Unit Tests as there is no effective way to test this unit without relying almost entirely on the implementation specifics.
The CQRS use cases are currently also not tested, the use cases currently are basically just forwarding the request through the DDD layers. If more complex use cases are added that do execute any business logic then these should be tested appropriately.

Currently, this project only uses Unit Tests, so we're not testing the Presentation layer either.
This could be covered by some form of Service or End-to-end tests but that is out of scope for now.

