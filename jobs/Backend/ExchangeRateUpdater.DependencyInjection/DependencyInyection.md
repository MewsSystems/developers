# Dependency Injection Design
## Purpose
This project encapsulates the dependency injection configuration for the exchange rate services. Instead of registering all services directly inside a specific host (for example, the Web API), the setup is extracted into a shared extension method that can be reused by multiple applications.

The main entry point is the AddExchangeRateServices(IServiceCollection services) extension method, which wires the IExchangeRateProvider implementation and the CNB HTTP client together with basic resilience policies.

## Why a separate project?
There are several reasons for keeping this registration logic in a dedicated project rather than placing it directly inside the API project:

- **Separation of concerns**\
The host projects (API, console, etc.) focus on hosting concerns (endpoints, input/output, configuration), while this project focuses only on how the exchange rate domain and infrastructure are composed. This makes the startup code of each host simpler and easier to read.

- **Future extensibility**\
If in the future a new host is introduced (for example, a SOAP service, a Windows service, or a background worker), it can reuse the same AddExchangeRateServices method without needing to replicate the registration logic.

Centralizing this setup ensures that all consumers benefit from the same resilience configuration and that changes (for example, different retry settings or a different client implementation) can be made in a single place.