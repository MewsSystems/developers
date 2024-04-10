
 # Architecture
 
 The exercise contained only a console application so I decided to divide it in two layers: a frontend as a console and a backend that caches data from the Czech National Bank exchange server.

 - The backend is an ASP.NET application ([ExchangeRates.Api](./ExchangeRates.Api/Program.cs)) that follows a Clean Architecture and some concepts of DDD.
 - The frontend is a console application ([ExchangeRateUpdater](./ExchangeRateUpdater/Program.cs)) following the Vertical Slice architecture.
 
 The backend consists in several projects (the ones that start with 'ExchangeRates') and the frontend is just one project 'ExchangeRateUpdater'.

 # Backend
 
 - DTOs as record classes (copyable and immutable) and placed them in their own project called [ExchangeRates.Contracts](./ExchangeRates.Contracts).
 - Use of the mediatr library to separate requests into commands and queries ([ExchangeRates.Application](./ExchangeRates.Contracts)).
 - I created an error middleware ([ErrorController.cs](./ExchangeRates.Api/Controllers/ErrorController.cs)) for translating exceptions into HTTP Problem Details.
 - The backend caches data with the decorator design pattern (see Infrastructure [CacheExchangeRateRepository.cs](./ExchangeRates.Infrastructure/Repositories/CacheExchangeRateRepository.cs)).
 - I use Options for the URL and configuration.
 - I configured the HTTP client in [ExchangeRatesRepository.cs](./ExchangeRates.Infrastructure/Repositories/ExchangeRatesRepository.cs) with retries with the Polly library, see [ServiceCollectionExtensions.cs](./ExchangeRates.Infrastructure\DependencyInjection\ServiceCollectionExtensions.cs).
 - I created a request URI builder for URL encoding query parameters (see [RequestUriBuilder.cs](./ExchangeRates.Http/RequestUriBuilder.cs)).
 - I used the Fluent interface quite a lot for the Dependency Injection. (see any DependencyInjection folder).
 
 # Frontend

 - The frontend is composed of a command line argument [parser](./ExchangeRateUpdater/DependencyInjection/Parser.cs) and several [operations](./ExchangeRateUpdater/Features), one of them being the exchange rate reader.
 - The idea is to add operations easily in the future with the use of the [IOperation](./ExchangeRateUpdater/Common/IOperation.cs) interface, which follows the strategy pattern.
 - With Vertical Slice architecture we have all layers inside each feature, see [operations](./ExchangeRateUpdater/Features/GetExchangeRateOperation.cs).
 - The frontend consists on two features, showing the help message and getting a list of exchange rates.
 - The list of currencies to show is passed as an argument (see the [launchSettings.json](./ExchangeRateUpdater/Properties/launchSettings.json)):
```bash
> ExchangeRateUpdater.exe --exchange_rates USD EUR CZK JPY KES RUB THB TRY XYZ
```

 # Automated Tests

  - See the Tests folder.
  - I did some unit tests and an [integration test](./Tests\ExchangeRates.Api.Tests.Integration) of a whole controller using the WebApplicationFactory.
  
 # Notes

 - Finding the API was a bit difficult. In the end I searched in google 'czech national bank exchange rate api swagger' and found a post with the url:
 https://developers.cnb.cz/discuss/633c52c8da0e79003c33a2c1
 - Exchange rates are of type 'decimal' because we deal with exact values (bit-data vs floating-point rounding errors), smaller range better precision.

# Pending

- The cache decorator is not ideal because you would need to create a new one for each different repository, so it would be nice to implement it as a generic type, for example
creating a generic repository like in the [Ardalis]( https://github.com/ardalis/Specification/blob/main/Specification/src/Ardalis.Specification/IReadRepositoryBase.cs#L15)
library.
- Documentation of public classes, methods and variables.
- Pagination of the exchange rates (if we were to show them to the user) as the response returned by the backend is quite big.
- HTTP retries with Polly: implemeneting a Circuit Breaker with Exponential Backoff, for example.
