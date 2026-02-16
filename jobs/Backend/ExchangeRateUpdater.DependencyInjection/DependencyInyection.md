# Dependency Injection Design

## Purpose

This project encapsulates the dependency injection configuration for the exchange rate services. Instead of registering all services directly inside a specific host (for example, the Web API), the setup is extracted into a shared extension method that can be reused by multiple applications.

The main entry point is the `AddExchangeRateServices(IServiceCollection services)` extension method, which wires together:

- The `IExchangeRateProvider` and `IExchangeRateService` implementations.
- The CNB HTTP client (`IExchangeRateClient`) configured with a basic resilience policy (retries with exponential backoff).

## Why a separate project?

There are several reasons for keeping this registration logic in a dedicated project rather than placing it directly inside the API project:

- **Separation of concerns**  
  The host projects (API, console, etc.) focus on hosting concerns (endpoints, input/output, configuration), while this project focuses only on how the exchange rate domain and infrastructure are composed. This makes the startup code of each host simpler and easier to read.

- **Reusability across hosts**  
  Any application can call the same `AddExchangeRateServices` method to get a fully configured exchange rate stack without duplicating registration code.
