# Mews backend developer task

# 💱 Exchange Rate Updater
A .NET 8 console application that fetches and caches exchange rates from the Czech National Bank (CNB), using Quartz.NET for scheduled execution and Polly for resilient HTTP communication. A pragmatic approach combined with OOP and SOLID principles has been taken that particularly focuses on human-readable and intuitive code with modularity, reliability and ease of future extension.

## TLibraries and Dependencies
| Component              | Purpose                                 |
|------------------------|-----------------------------------------|
| .NET 8                 | Core framework                          |
| HttpClientFactory      | Typed API client                        |
| Polly                  | Retry and timeout policies              |
| Quartz.NET             | Job scheduling                          |
| MemoryCache            | In-memory caching of exchange rates     |
| Microsoft.Extensions   | Hosting, Logging, Configuration         |
---

## Main Features
- **CNB API integration**
  - Typed HTTP client with configurable headers
  - Normalizes rates per 1 unit
- **Caching**
  - Avoids redundant API calls
  - Skips invalid currencies
  - Combines cached and fresh results
- **Modular Architecture**
  - Hosted DI setup via `ServiceCollectionExtensions`
  - Easily extendable to other exchange rate providers with generics
- **Structured Logging**
  - Logs API calls, retries, missing currencies and job execution

- **Quartz.NET Scheduled Jobs**
  - Runs once immediately on startup to catch current exchange rates
  - Runs daily at **14:30:30 CEST** (weekdays)

  The `ExchangeRateRefreshJob` runs on startup and at 14:30:30 CEST weekdays:

  ```csharp
  q.AddTrigger(opts => opts
    .ForJob(jobKey)
    .WithIdentity("ExchangeRateRefreshTrigger")
    .WithSchedule(CronScheduleBuilder
        .CronSchedule("30 30 14 ? * MON-FRI")
        .InTimeZone(TimeZoneInfo.FindSystemTimeZoneById("Central Europe Standard Time"))));
    ```

- **Retry Policy with Polly**
  - Retries transient failures and timeouts
  - Handles rate limits and `Retry-After` headers
  - Adds jitter and exponential backoff

  ```csharp
  .WaitAndRetryAsync(
    3,
    retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)) + jitter,
    onRetry: LogRetry);
  ```

## Getting Started

1. Clone the repo
2. [Configure `appsettings.json`](#configuration) with CNB API details and a list of ISO currency symbols
3. Run the app:

```bash
dotnet run
```

You must be connected to the internet, you’ll be presented with:

- Exchange rates printed to console
- Logs for API calls, retries, and job execution

  <img width="730" height="313" alt="image" src="https://github.com/user-attachments/assets/f2a5ad93-9d2a-41b4-bf9c-819ae9e21361" />


## Configuration

Example `appsettings.json`:

```json
{
  "ExchangeRateConfiguration": {
    "CurrencyCodes": [ "USD", "EUR", "GBP" ]
  },
  "ApiConfiguration": {
    "Name": "CNB",
    "BaseUrl": "https://api.cnb.cz",
    "ExchangeRateEndpoint": "exrates/daily",
    "Language": "EN",
    "RequestTimeoutInSeconds": 20,
    "RetryTimeOutInSeconds": 5,
    "DefaultRequestHeaders": {
      "Accept": "application/json"
    }
  }
}
```

## Potential Future Enhancements
- Add more tests, especially integration and end-to-end tests
- Add persistent caching, preferably distributed (e.g. Redis)
- Add new providers by implementing `IApiClient<T>`
- Expose rates via REST or gRPC
- Add health checks or metrics
- Add CD pipeline for automated deployment