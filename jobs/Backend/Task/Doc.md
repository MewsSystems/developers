# Mews - Backend Technical Test

This is the backend solution of Mews technical test.

# Technical details

* Exchange rates data source: https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates
* Main technical approaches used:
** Clean Architecture design has been applied to ensure a separation of concerns across the system.
*** The Exchange Rate provider has been designed as a use case which is not depending on any specific bank exchange rate or data access.
** Inversion of control - Dependency injection.
** Command Query Responsibility Segregation (CQRS).

* Tests: main unit tests provided to cover both successful and failure paths. The ExchangeRateUpdater console app has been used as manual test to check integration.