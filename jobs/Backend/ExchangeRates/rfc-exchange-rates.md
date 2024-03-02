# RFC 001- Implement exchange rates

This is the RFC to enable retrieving exchange rates from the external API of CNB.

| RFC #     | 001                   |
| --------- | --------------------- |
| Date      | 2024-03-01            |

# Summary

This feature will introduce a way to retrieve exchange rates from the external API of Czech National Bank. This proposal covers the MVP implementation, and the future improvements can be done once we know more about the usage of the feature.

# Proposal

We are going to introduce a new ExchangeRateProvider that will be responsible for retrieving the exchange rates. The Client implementation will be focused around CNB API https://api.cnb.cz/cnbapi/swagger-ui.html#/. 
At the moment we are ineterested in retrieving only daily rates ('https://api.cnb.cz/cnbapi/exrates/daily?lang=EN') which are updated after 14:30 CET daily.
In order to avoid calling the API multiple times, we are going to introduce a caching mechanism (Redis). The cache will be stored in the in memory database and will be updated daily after the new rates are available. 
As this API is a third party dependency, we have to assume that there might be errors.

## Alternatives considered

The requirements of this feature mentioned only support of the CNB. However if we see that we need to support other providers in the future, we might consider integrating with a third party service that provides exchange rates from multiple sources. E.g.https://www.fluentax.com/

## How does it work now?

There is a list of currencies that we want to see the exchange rates for. The program calls provider and prints the exchange rates.

## What are we changing?

We are aimimg at the real minimum viable product. We are going to implement the minimum changes required:
- We implement CNB client.
- We implement caching mechanism.
- The supported base currency is CZK. All exchange rates are calculated from CZK.
- We implement error handling. 

## How are we changing it?

- We use Redis to store the exchange rates.
- We use the CNB API to retrieve the exchange rates.

**Risks to consider here**

- There are a lot of unclear requirements, which may cause future changes to the implementation, depending on load, performance limits, limitations of CNB API.
- We do not store the list of supported currencies as of yet, so any not cached currency will cause an attempt to retrieve it from the API.

### Measuring the results

- N/A
