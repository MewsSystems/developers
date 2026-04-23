# Mews Assignment

This document briefly describes how the solution for Backend DotNet assignment works and what I could have done better.

The implementation uses **Ports&Adapter** and some concepts from **Domain Driven Design**.

# Host

## **General API Overview**

The start of the program is under the **ExchangeRateUpdater.Host.WebApi** which is de facto a small API that calls under the hood two endpoint of Czech National Bank. Once you run the solution, the Swagger UI can be accessed by simply accessing the root route. The API contains of one controller and two endpoints:

- **GetAllFxRatesAsync endpoint** - will query Czech National Bank for all foreign exchange rates for a certain date. Date is passed as an optional query parameter. If the query parameter is missed the current time of the machine on which the API runs is considered.
- **ExchangeAsync endpoint** - will compute the exchange converted sum of between the source and target currencies taking the latest exchange rate for the mentioned currencies by querying Czech National Bank.

## API settings

Api settings are read from `settings.yaml`  and converted to a `ISettings` interface. For logging purposes **Serilog** logger is used. The logs are written by default to Console. For structured logging, since it is an API, the default properties would be **CorrelationId** and request path/query path.

# Domain

Domain is where the business logic lives and is structured in Entities, Value Objects, and UseCases.

- For **Value Objects**, we define Currency as a ISO 4217 approved code and Positive Real Number - there are no negative exchange rates.
- For **Entities**, we handle the definition of ExchangeRate, ExchangeOrder, and ExchangeResult.
- **UseCases** - simply consists of the ExchangeOrderUseCase.

# Adapters

Adapters are “external”(to the Domain) dependencies that are used in the domain by means of a port implementation.

## Adapter.ExchangeUpdateProvider.InMemory

- **ExchangeRateProviderRepositoryInMemory** - an in-memory representation of an exchange provider. It is meant to be used in test most of the time. It is mainly used in unit tests.
- **ExchangeRateCacheRepositoryInMemory** - a in-memory representation of a cache. There are a few important points to mention about cache design:
    - **LRU cache eviction** - My assumption would be that the most most recent accessed values would be for the latest days. Hence it makes sense to evict the days that are no longer queried.
    - **TTL setup** - As far as we know, the fx rates for the current date will be published after 14:30(on the moment of writing this Readme - 07.12.2023). The past dates fx rates do not really change, so the TTL for those dates does not really matter that much. For the demonstration purposes I will set it to 24 hours. We need to have a strict TTL for the current day. I chose a TTL of 1 minute. This TTL has its minuses and pluses. We have a chance to have wrong data for only 1 minute. This rapidly changing TTL will result in a way more requests to the Czech National Bank. The 1 minute of wrong data, we can compensate with showing our customer the date of the exchange rate. A future improvement would be to play with cache around the 14:30 time. Maybe once we get the data in for the current date we can increase the cache TTL as well.

## Adapter.ExchangeUpdateProvider.CzechNatBank

**CzechNationalBankRepository** is the repository that handles http calls to Czech National Bank and then passes a stream to **ExchangeRatesTextParser. ExchangeRatesTextParser** parses the txt document presented as a stream and converts the parsing to specific dtos. Http calls are wrapped around with **Polly.** For the purpose of this API, we will have only 2 retries(3 executions), but of course, we can further change the sleep times as well as the number of retries.