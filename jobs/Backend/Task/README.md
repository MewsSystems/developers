# Code Challenge – CNB Exchange Rate Provider

This pull request implements a solution for retrieving exchange rate data from the Czech National Bank (CNB) web services.

---

## ✅ Requirements Implemented

- **ExchangeRateProvider**:
  - Fetches daily exchange rates from the CNB JSON API.
  - Filters results based on requested currency codes.

---

## 🔍 Data Source Analysis

The CNB provides daily exchange rates in three formats:

- **JSON**: `https://api.cnb.cz/cnbapi/exrates/daily?date=YYYY-MM-DD&lang=EN`
- **Text**: Plain text format.
- **XML**: Standard XML with metadata.

> **Chosen Format**: The JSON API was selected for this implementation due to its modern structure and ease of parsing via `.NET`’s `System.Text.Json`.

> **Authentication**: None of the CNB endpoints require authentication.
---

## 🕒 Exchange Rate Update Schedule

- **Update Time**: On working days after **14:30 local time** (no guaranteed exact time).
- **No Updates**: On weekends and **Czech public holidays**.
- **Update Detection**:
  - JSON: `validFor` field
  - Text: First line (e.g., `19.09.2025 #183`)
  - XML: Root attribute `date` (e.g., `<kurzy date="19.09.2025" ... />`)

> Note: The system determines the next working day programmatically, considering weekends and Czech national holidays.

---

## 🧱 Architectural Approach

### Clean Architecture

The project follows Clean Architecture principles, separating responsibilities across the following layers:

- **Presentation**:
  - `Api`: RESTful interface for consumers
  - `ConsoleApp`: Lightweight CLI tool for debugging or testing
- **Application / Domain / Infrastructure**:
  - Encapsulates core logic, abstractions, and external dependencies

### Benefits

- **Testability**: Interfaces are used throughout, enabling mocking.
- **Maintainability**: Clear separation of concerns.
- **Extensibility**: Components can be replaced or extended with minimal impact.

---

## 🌐 CNB Integration

- **Typed HTTP Client** is used for structured, reusable access.
- **Deserialization**: Handled by `System.Text.Json.JsonSerializer`.
- **Resilience**: Powered by **Polly** with retry policies for transient failures.

---

## 🧠 Caching Strategy

To optimize performance and respect the CNB’s update cadence:

- **Initial Request**: Triggers data fetch and in-memory cache population.
- **Cache Expiry**:
  - Set to 14:30 on the next working day.
- **Post-14:30 Logic**:
  - After 14:30, the system checks for updates every 10 minutes until the `validFor` value changes.

### Key Benefits

- Reduces unnecessary API calls
- Ensures timely refreshes once new data becomes available
- Enables consistent and predictable refresh cycles

> **Note**: In-memory caching is used for simplicity. This can be swapped with a distributed cache like Redis for scalability.

---

## 🧰 Middleware

### ExceptionHandlingMiddleware

Centralized error handling that:

- Catches unhandled exceptions
- Returns standardized error responses
- Prevents internal details from leaking
- Logs exceptions for diagnostics

### RequestTimingMiddleware

- Logs the duration of each request for observability

---

## 📝 Logging

Key application events are logged, including:

- Source of data (cache vs CNB API)
- Current `validFor` value
- Next expected refresh time
- Errors or retry attempts (via Polly)

---

## 🔄 Dependency Injection

The project uses **.NET’s built-in Dependency Injection** system.

### Composition Root Strategy

- Each layer registers its dependencies via extension methods.
- Shared configuration is centralized and reused across both the API and Console apps.

### Examples of Registered Services

```csharp
services.AddTransient<IExchangeRateProvider, ExchangeRateProvider>();
services.AddHttpClient<ICnbApiClient, CnbApiClient>();
services.AddMemoryCache();
```

### Application Entry Points

- **API**: Configured via Program.cs using standard WebApplicationBuilder.
- **ConsoleApp**: Uses HostBuilder to replicate the same DI setup, ensuring full parity between environments.


## 🧪 Unit Testing

The project includes unit tests organized by layer:
- Follows the same structure as the production code for easier traceability.
- Each test project targets:
  - Domain logic
  - Application services
  - Infrastructure adapters
- Uses appropriate mocking (e.g., Moq) to isolate behaviors.
  - Test Frameworks:
    - xUnit
    - Moq (for mocking dependencies)
    - FluentAssertion

---
## ✅ Summary

This implementation:
- Meets all functional requirements.
- Follows modern .NET and Clean Architecture best practices.
- Is resilient, testable, and ready for extension (e.g., distributed caching, fallback APIs, scheduled jobs).
