# ExchangeRateUpdater

**Time spent:** ~3–4 hours

A clean, modular .NET application that retrieves and processes daily exchange rates from the **Czech National Bank (CNB) API**.

## Key Design Decisions

- **Dependency Injection:** All dependencies are resolved via the built-in .NET DI container for better testability and decoupling.
- **Options Pattern:** Strongly typed configuration validated at startup to ensure correctness.
- **Error Handling:** Structured logging with `ILogger<T>` and fail-fast validation to ensure robustness.
- **HttpClientFactory:** Centralized, typed HTTP client setup for reliability and reuse.
- **External Integration:** Uses the official CNB API — [https://api.cnb.cz/cnbapi/swagger-ui.html](https://api.cnb.cz/cnbapi/swagger-ui.html)


## Summary

- SOLID-based, clean architecture  
- Clear separation of concerns  
- Strong configuration and logging setup  
- Extensible for additional providers or data sources  
- Fully testable through abstraction and DI  


## Potential Improvements

- Replace console app with an API or background worker service for continuous rate updates  
- Add caching or persistence for performance under heavy load  
- Implement unit tests for service and provider layers  

