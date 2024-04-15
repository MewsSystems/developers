# Mews - Backend Technical Test

This is the backend solution of Mews technical test.

# Technical details

* Main technical approaches used:
** Clean Architecture design has been applied to ensure a separation of concerns across the system.
*** The Exchange Rate provider has been designed as a use case which is not depending on any specific bank exchange rate or data access.
** Inversion of control - Dependency injection.
** Command Query Responsibility Segregation (CQRS).

* Tests: main unit tests provided to cover both successful and failure paths. The ExchangeRateUpdater console app has been used as manual test to check integration.

* Functionality: 
** The application gets the rate exchanges for those specified currencies meeting the following conditions:
*** the currency can be found via cnbapi/exrates/daily: https://api.cnb.cz/cnbapi/swagger-ui.html#/%2Fexrates
*** the currency amount is 1 so that the business logic does not have to calculate the rate as per current task requirements. 

* To improve:
** Explore other exrates endpoint options to improve the exchange rate data retrieval and find more currency from source.
** Extend logging across the application to improve observability/monitoring.
** Add configuration.
** Add integration testing.
** Increase code coverage with unit tests, for example, testing mapping classes.
** Add factory class to create IFinancialClient based on target currency and make it configurable. Currently the system supports CZK and Czech National Bank API only.
** Improve resiliance by considering retry mechanisms to mitigate network issues.
** Consider caching to increase performance and reduce network traffic and potential network issues.
** Add Mediator pattern to redirect the exchange rate requests to the application.