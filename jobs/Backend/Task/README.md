# Mews.ExchangeRateUpdater

A modular and maintainable application that fetches and serves foreign exchange rates from the Czech National Bank (CNB), following Clean Architecture principles with clear separation of concerns across layers.

The **presentation layer** includes two projects:

  - **API**: A minimal Web API that serves stored exchange rates. It avoids hitting the CNB on every request by exposing cached data.
  - **Fetcher**: A background service that runs once daily at **13:30 UTC**, shortly after CNB publishes the day’s exchange rates, fetching the latest rates from CNB. It uses **Polly** for retries (every 5 mins till it works). If rates already exist for the day, it logs and skips silently.

These components were not required by the original technical task but have been added as lightweight, practical examples of how to consume the core logic. The API is intentionally kept simple with minimal hosting logic instead of full controller-based MVC.

This design improves **reliability** and **resilience** by acting as a buffer in case the CNB site is temporarily unavailable. However, the system will only serve exchange rates for the current day. If today's rates could not be fetched (e.g., due to a persistent CNB outage), the API will return an error rather than serve outdated data, even if rates from the previous day are available.

All logs are enriched with a unique **TraceId** per request or fetch run, enabling full traceability across all layers. Check the logs for the TraceId to correlate related operations.

---

## Solution Architecture

The solution is composed of 8 projects:

### 1. Presentation
- **Mews.ExchangeRateUpdater.API**  
  A minimal Web API that exposes endpoints to get exchange rates and force updates. Uses Serilog with TraceId for structured logging.

- **Mews.ExchangeRateUpdater.Fetcher**  
  A background worker (hosted service) that periodically fetches exchange rates and stores them. Useful for automation or cron-like jobs.  
  Currently, it simply waits until the next day to perform the next fetch.

### 2. Infrastructure
- **Mews.ExchangeRateUpdater.Infrastructure**  
  This layer depends only on `Domain` and interface contracts. It implements low-level technical concerns, including:
  - `Persistance`: EF Core implementation of `IExchangeRateRepository`
  - `Services`: HTTP client for CNB `CnbService` and a separate response parser `CnbParser` to convert CNB JSON into domain entities.
  - `Interfaces`: DI ports like `ICnbService`
  - `Logging`: Serilog enrichers
  - `ServiceCollectionExtensions`: Registers infrastructure services and dependencies for DI.

### 3. Application
- **Mews.ExchangeRateUpdater.Application**  
  This layer contains the core business logic and application use cases. It orchestrates domain operations and encapsulates the behavior exposed to the outer layers (e.g., APIs, background workers). It depends only on the Domain layer and abstractions (interfaces), maintaining the principles of Clean Architecture.
  - `UseCases`: Encapsulate the main business flows (`FetchExchangeRatesUseCase`, `GetExchangeRatesUseCase`).
  - `Interfaces`: Define contracts to be implemented by external services (`IFetchExchangeRatesUseCase`, `IGetExchangeRatesUseCase`).
  - `Exceptions`: Domain-related application exceptions to control business rules and flow.
  - `ServiceCollectionExtensions`: Registers application services and dependencies for DI.

### 4. Domain
- **Mews.ExchangeRateUpdater.Domain**  
  Holds pure domain models (`ExchangeRate`, `Currency`) and abstractions (`IExchangeRateRepository`, `ICnbService`). No dependencies.

### 5. Tests
- **Mews.ExchangeRateUpdater.Domain.UnitTests**  
  Domain ValueObject tests (e.g., Currency equality, ExchangeRate formatting)
- 
- **Mews.ExchangeRateUpdater.Application.UnitTests**  
  Unit tests for the use cases.

- **Mews.ExchangeRateUpdater.Infrastructure.UnitTests**  
  Unit tests for infrastructure components like CnbService, with mocked HTTP responses.

- **Mews.ExchangeRateUpdater.Infrastructure.IntegrationTests**  
  Tests that involve external dependencies (real DB, HTTP client).

---

## Running the solution

### Requirements
- Docker & Docker Compose
- Make
- dotnet 8

### Start all services in Docker containers
```bash
make up
```

This will start:
- `exchange-rate-updater-api` on `http://localhost:8080/swagger`
- `exchange-rate-updater-fetcher` worker service that fetches daily rates into the DB
- SQLite DB mounted in volume `db-data`

### Available Makefile commands

| Command                  | Description                            |
|--------------------------|----------------------------------------|
| `make help`              | Show help message with all commands    |
| `make up`                | Start all services with Docker Compose |
| `make rebuild`           | Rebuild and start all containers       |
| `make down`              | Stop and remove containers/volumes     |
| `make logs`              | Tail logs from Docker                  |
| `make test-unit`         | Run unit tests                         |
| `make test-integration`  | Run integration tests                  |
| `make run-local-api`     | Run API locally with dotnet            |
| `make run-local-fetcher` | Run Fetcher locally with dotnet        |


**Note**: When running locally (outside Docker), the SQLite DB file is stored in the following path depending on your OS:
  - **WINDOWS:**
  ```shell
  C:\Users\<your_user>\AppData\Local\ExchangeRateUpdater\app.db
  ```
  - **Linux:**
  ```shell
  /home/<your_user>/.local/share/ExchangeRateUpdater/app.db
  ```
  - **MacOS:**
  ```shell
  /Users/<your_user>/Library/Application Support/ExchangeRateUpdater/app.db
  ```

---

### API Endpoints
- `GET /rates?currencies=USD&currencies=EUR`: Get today’s exchange rates
- `PUT /rates`: Fetch and store today’s CNB rates.  
  Request body:
  ```json
  {
    "force": true
  }
  ```

---

## Tests

### Unit Tests
Run from test projects:
```bash
make test-unit
```

### Integration Tests
```bash
make test-integration
```

---

## Patterns & Practices

- **Clean Architecture**: Domain-centered design with dependencies flowing inward.
- **Dependency Injection**: Used across all projects with interfaces in `Domain`.
- **Logging**: Structured logs using Serilog with `TraceId` per request/worker run.
- **Exception vs Result**: Business flow errors are being migrated from exceptions to `Result<T>` pattern.
- **Repository Pattern**: Abstraction over data access (IExchangeRateRepository) with a clean separation from EF Core.
- **Interface Segregation**: Interfaces are small and focused (e.g., ICnbService).
- **Separation of Concerns**: Clear boundaries between fetching, storing, exposing, and parsing data.
- **Trace Context Propagation**: A TraceId is generated per request (API) or execution (Fetcher) and propagated through all layers to enable log correlation and end-to-end observability. Logs are enriched with TraceId using Serilog enrichers.
- **Value Objects**: Currency and ExchangeRate are modeled as immutable value objects for correctness and equality comparison.

---

## Data
- Database: SQLite, persisted in volume `db-data`.
- Schema is auto-created on startup.
- Stored data: Date, SourceCurrency, TargetCurrency, Value.
- Source: [CNB Exchange Rate API](https://api.cnb.cz/cnbapi/)


---

## Volumes
- `db-data`: Persistent SQLite database
- `logs`: Logs
