# Mews ExchangeRateUpdater application

This application fetches a list of exchange rates provided by Czech National Bank, and displays them.

Possible points of improvement.

* Add an in-memory cache to ExchangeRateRepository.
* Configure RestEase to enable resiliency. (Timeouts, retries, etc..)
* Implement additional unit tests for Core.Extensions, Core.Domain, Core.Application
* Diversify test scenarios for Core.Infra
* Improve on functional programming concepts
