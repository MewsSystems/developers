# Exchange Rate Provider - Modular & Extensible System

This project introduces a modular system for retrieving and managing exchange rates from multiple countries, with a focus on clean architecture, environment configuration, and maintainability. The initial implementation supports only the **Czech Republic**, but is fully extensible to other countries.

---

## Architecture Overview

A **factory pattern** has been introduced to encapsulate the logic of retrieving exchange rates per country. Currently, only the Czech Republic (CZE) is implemented, but support for other countries can be easily added by implementing the `IExchangeRateProvider` interface.

---

## Dependency Injection

The project has been updated to use dependency injection for all services and components, enabling better testability, separation of concerns, and integration with ASP.NET Core or other DI containers.

---

## Logging

Logging has been configured by default to output to the console. This configuration is environment-specific and can be adjusted for other targets such as files, cloud logging platforms, or databases depending on the deployment context.

---

## Environment Configuration

Environment-specific configuration has been implemented, starting with the `Development` environment. The `appsettings.Development.json` file includes:

- Minimal log configuration
- Exchange rate provider settings (currently for CZE only), including:
  - Source URL
  - Minimum cache duration
  - Local update time
- Cache configuration (in development, a JSON file-based cache is used)

For production environments, the configuration can be easily adapted to use more robust systems such as **Redis** for caching.

---

## Caching

A caching layer has been added to reduce the number of calls to the **Czech National Bank (CNB)** API. This prevents unnecessary requests and conserves network bandwidth. Cached values are stored with timestamps to determine validity based on the update time.

---

## Time Management (UTC Standardization)

To handle time zones correctly and consistently across environments, all timestamps are transformed and stored in **UTC**. This standardization helps ensure consistency in time comparisons, regardless of local server configuration.

A custom interface `IDateTimeProvider` has been introduced to abstract UTC time retrieval, aiding unit testing and mocking.

Because the `appsettings` uses **local time** strings (e.g., `"08:00:00"`), the [TimeZoneConverter](https://www.nuget.org/packages/TimeZoneConverter) NuGet package has been installed. This enables reliable conversion of the time zone `"Central European Standard Time"` into platform-compatible `TimeZoneInfo` objects, ensuring support across Windows and Linux environments.

---

## Unit Testing

Unit tests have been added to validate core functionality such as:
- Time zone conversions
- Cache validity checks
- Factory provider resolution
- Rate retrieval and mapping

These tests ensure the system behaves correctly and can be safely extended.

---

## Adding a New Country

To support a new country's exchange rates:

1. **Add it to the enum**:  
   Extend `CountryIsoAlpha3` with the new country code.

2. **Implement the provider**:  
   Create a new class that implements `IExchangeRateProvider`.

3. **Register in the factory**:  
   Add your new implementation to the `ExchangeRateProviderFactory`.

4. **Add configuration**:  
   Extend the `ExchangeProviders` section in `appsettings.*.json` with required fields such as:
   - URL
   - Cache settings
   - Update schedule

5. **Create settings model**:  
   Define a `Settings` class matching the configuration structure for the new country.

6. **Register services**:  
   Add service registration in the `AddExchangeProviders` method or module.

---

## Project Highlights

-  Factory pattern for country-specific logic  
-  Dependency injection support  
-  Configurable per environment  
-  Unified UTC time handling  
-  Efficient caching  
-  Unit-tested architecture  
-  Easily extensible for additional countries

---


##  Conclusion

This architecture enables scalable and environment-aware handling of exchange rates for multiple countries, promoting clean design and operational flexibility. By adhering to SOLID principles, the system is easy to extend, test, and configure for real-world use.

