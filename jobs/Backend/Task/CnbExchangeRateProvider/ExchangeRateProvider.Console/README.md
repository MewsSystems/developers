# CNB Exchange Rate Provider

This project implements an `ExchangeRateProvider` for the Czech National Bank (CNB) to retrieve daily exchange rates from their official public data source. The solution is designed with production-grade considerations in mind, focusing on maintainability and robustness.

## Data Source

The exchange rate data is sourced from the official CNB API:
`https://api.cnb.cz/cnbapi/exrates/daily`

## Project Structure

*   `CnbExchangeRateProviderApp.csproj`: The .NET project file.
*   `ExchangeRateProvider.cs`: Contains the core logic for fetching and parsing exchange rates.
*   `Program.cs`: A sample console application to demonstrate the usage of the `ExchangeRateProvider`.

## How to Build and Run

1.  **Navigate to the project directory:**
    ```bash
    cd CnbExchangeRateProvider/CnbExchangeRateProviderApp
    ```

2.  **Restore dependencies:**
    ```bash
    dotnet restore
    ```

3.  **Build the project:**
    ```bash
    dotnet build
    ```

4.  **Run the application:**
    ```bash
    dotnet run
    ```

    The application will output the obtained exchange rates to the console.

## Production-Grade Considerations

### Error Handling

The `ExchangeRateProvider` includes robust error handling to manage various scenarios:

*   **Network Issues (`HttpRequestException`):** Catches errors that occur during the HTTP request to the CNB server.
*   **XML Parsing Errors (`XmlException`):** Handles cases where the downloaded data is not a valid XML format.
*   **Missing or Invalid Data:** Includes checks within the XML parsing loop to gracefully handle missing attributes or elements, and unparseable rate/amount values, logging warnings to the console.

In a production environment, these exceptions should be logged using a structured logging framework (e.g., Serilog, NLog) with appropriate alerting mechanisms.

### Caching

For a production system, caching the exchange rates is highly recommended to:

*   Reduce the load on the CNB's server.
*   Improve application performance by avoiding redundant network requests.
*   Provide resilience in case of temporary network or CNB API issues.

Since exchange rates are typically updated daily, a caching strategy with a daily refresh interval would be appropriate. This could be implemented using `IMemoryCache` in ASP.NET Core, or a custom caching solution.

### Logging

Currently, `Console.WriteLine` is used for output and basic error messages. In a production environment, a dedicated logging solution should be integrated. This would allow for:

*   Configurable log levels (Debug, Info, Warning, Error, Critical).
*   Structured logging for easier analysis.
*   Output to various sinks (files, databases, cloud logging services).

### Dependency Injection

For better testability and maintainability, the `HttpClient` should be injected into the `ExchangeRateProvider` using Dependency Injection. This allows for easier mocking of `HttpClient` in unit tests and better management of its lifecycle.

### Unit Testing

Comprehensive unit tests should be developed to cover:

*   Successful retrieval and parsing of exchange rates.
*   Handling of malformed XML responses.
*   Handling of network errors.
*   Edge cases for currency data (e.g., missing code, zero amount).

## Future Enhancements

*   **Date-specific rates:** The current implementation fetches daily rates. Enhancements could include fetching historical rates or rates for a specific date if the CNB API supports it.
*   **Configuration:** Externalize the CNB API URL and other magic strings into a configuration file (e.g., `appsettings.json`).
*   **Abstraction:** Introduce an interface for `IExchangeRateProvider` to allow for different implementations (e.g., a mock provider for testing, or a provider for a different bank).
