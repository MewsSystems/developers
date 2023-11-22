# Mews Backend Developer Task (.NET)

## Goal

Implement the Exchange Rate Service using .NET Core technology to fetch daily exchange rates from a Czech National Bank API.

## Solution Structure

In the documentation folder, we have a readme file with an overall project description.

Inside the src folder, we have the following .NET projects:

- **Mews.ExchangeRateProvider.Api:** This is a WEB API project providing an endpoint for clients to call. When fetching rates, we enable optional parameters for date, language, and the option to get all rates or just the ones of interest.
- **Mews.ExchangeRateProvider.Application:** This is a class library providing an interface for the API project to call.
- **Mews.ExchangeRateProvider.Infrastructure:** This is a class library providing interfaces for the Application project to call. It contains the CNB client implementation, caching, etc.
- **Mews.ExchangeRateProvider.Domain:** This is a class library containing data objects (entities, DTOs) used by all other projects.

Inside the test folder, we have the following test projects:

- **Mews.ExchangeRateProvider.Api.Test**
- **Mews.ExchangeRateProvider.Application.Test**
- **Mews.ExchangeRateProvider.Infrastructure.Test**

## Technologies and Concepts Used

- **ASP.NET Core:** The project is built using the ASP.NET Core framework, providing a scalable and modular architecture.

- **Swagger:** Allowing human-readable API documentation.

- **HttpClientFactory:** HttpClientFactory provides several benefits over HttpClient, including automatic handling of lifetime management, pooling, and configuration.

- **Polly for Retry Policy:** Implements retry policies using the Polly library to handle transient errors, improving the application's resilience by allowing it to recover from temporary failures when calling external systems.

- **In-Memory Caching:** Leverages in-memory caching for caching exchange rate data locally, reducing the need for frequent API calls. Caching is used to improve response times and reduce the load on the third-party API; in this case, we can use simple in-memory cache.

- **Unit Tests using XUnit and Moq:** Enables writing tests with easy object mocking.

- **Clean Architecture:** Provides a separation of concerns and maintainability. Clean Architecture promotes testability, flexibility, and scalability by organizing code into distinct layers such as Domain, Application, Infrastructure, and UI, which are then grouped by features.

## TO-DO List

- Add more unit tests and E2E tests.
- Consider adding rate limiting to limit number of requests in certain period.
- Implement more robust Polly policies to cover more scenarios.
- Consider using Serilog for logging, providing more detailed logs.
- Utilize a scheduled job to clean the cache at a defined time so that the cache could be kept for one day (CNB updates exchange rates daily).




