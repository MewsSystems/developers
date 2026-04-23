# Exchange Rate Updater – Notes & Considerations

> **TL;DR**  
> A clean, immutable, and test-focused solution that fetches and parses daily CNB exchange rates, emphasizing correctness, validation, and separation of concerns

<br>


### _How to Run The Application_

```bash
# navigate to src/Application folder
cd src/Application

# restore dependencies
dotnet restore

# build the solution
dotnet build

# run unit tests
dotnet test

# run the application
dotnet run --project ExchangeRateUpdater.Application.csproj
```

<br>
<br>


## 1. Problem Statement

_See Challenge Instructions here: [Mews Backend .NET Challenge Instructions](https://github.com/MewsSystems/developers/blob/master/jobs/Backend/DotNet.md)_


The goal of this challenge is to fetch daily foreign exchange rates published by the Czech National Bank (CNB), parse the returned data, and expose the rates in a clean, domain-driven model.

The CNB endpoint returns exchange rates as a plain-text, pipe-separated file containing:
- A header with the publication date and working day number
- A column definition row
- One row per currency exchange rate

The solution is expected to:
- Retrieve the data over HTTP
- Parse and validate the response format
- Convert the raw data into strongly typed domain objects
- Handle invalid or unexpected input gracefully

<br>

## 2. Assumptions
The following assumptions were made to keep the solution focused and explicit:

- The CNB response format is stable and follows the structure found in the [sample_data.txt](sample_data.txt)
- All exchange rate values are positive
- The application aims to display the convertion of 1 unit of given currency. Example: 1 EUR -> CZK
- The target currency is implicitly CZK, as implied by the [CNB source](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt)
- The full response fits comfortably in memory
- This is a read-only, batch-style operation (no persistence required)

<br>

## 3. Approach
The solution is structured around a clear separation of concerns:

_It uses IoC and DI to allow new implementations to be added without breaking existing ones_

1. **HTTP Client Layer**
   - Responsible for retrieving raw data from the CNB endpoint
   - Configured with timeouts, retries, and resilience policies

2. **Parsing Layer**
   - Converts the pipe-separated text response into structured models
   - Validates headers, column definitions, and data rows and fails early in case the contract ever changes
   - Produces deterministic parsing results or meaningful exceptions

3. **Domain Layer**
   - Models currencies and exchange rates as immutable value objects
   - Enforces basic domain invariants (e.g. positive exchange rates)

4. **Application Layer**
   - Orchestrates the flow between client, parser, and domain
   - Exposes a clean API for consuming exchange rate data

The design favors clarity and correctness over premature optimization.

<br>

## 4. Design Decisions

### Immutability
- Domain entities and DTOs are implemented as immutable types
- Value-based equality is used where identity is defined purely by data
- This reduces side effects and simplifies reasoning and testing

### Separation of Concerns
- Parsing logic is isolated from HTTP and orchestration logic
- Domain models are independent of infrastructure concerns
- This makes individual components easier to test and evolve

### Explicit Validation
- The parser validates headers, column definitions, and row structure
- Failures are detected early and reported with clear exception messages

### Resilience
- HTTP calls are protected using retry and circuit breaker policies
- Transient failures are handled without leaking infrastructure concerns into the domain

<br>

## 5. Immutability & Modeling
The following modeling strategy was applied:

- **Records**
  - Domain value objects such as `Currency` and `ExchangeRate`
  - DTOs and parsing result models
  - Configuration objects loaded from application settings

- **Classes**
  - Services, clients, and parsers
  - Components that encapsulate behavior or orchestration

This distinction reflects the difference between *data* and *behavior* in the system.

<br>

## 6. Edge Cases & Error Handling
The solution explicitly handles:

- Empty or whitespace-only responses
- Missing or malformed headers
- Unexpected column definitions
- Invalid numeric values
- Incomplete or malformed data rows

When errors occur, the system:
- Fails fast during parsing
- Throws domain-specific exceptions with descriptive messages
- Avoids silently ignoring invalid data

<br>

## 7. Testing Strategy
The testing approach focuses on correctness and determinism:

- Unit tests for domain entities and invariants
- Comprehensive parser tests covering:
  - Valid inputs
  - Invalid formats
  - Edge cases and error scenarios
- Tests are isolated, fast, and do not rely on external systems

Unit tests for higher-level application behavior and Integration tests were intentionally omitted due to scope.

<br>

## 8. Limitations
Known limitations include:

- No caching of results between runs
- No dynamically parsing program args as currencies input
- No persistence layer
- No localization or formatting concerns addressed
- No retry backoff customization exposed via configuration

These were accepted trade-offs given the scope of the challenge.

<br>

## 9. Possible Improvements
With additional time, the following enhancements could be made:

- Add integration tests with a mocked HTTP server
- Implement better Domain Exceptions for proper retries / logging
- Implement greceful failures for malformed Exchange Rates 
- Consider adding caching
- Support additional rate providers via a common abstraction
- Expose richer observability (metrics, structured logs)