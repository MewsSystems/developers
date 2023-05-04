# 💵 Exchange Rate Provider - Solution

The implmemented solution contains following flow: Program -> ExchangeRateProvider -> ExchangeRateListingCache -> CnbApiClient -> CnbApiParser

- ExchangeRateProvider - retrieved all available exchange rates from cache and returns the ones relevant for requested currencies
- ExchangeRateListingCache - ensures that we don't call CNB api unnecessarily often so that it is fast (and CNB won't block us on suspicious traffic)
  - BankDateProvider - provides information about the date, when the latest available exchange rates listing should have been published (so that we do not request newer data for weekend days or national holidays)
- CnbApiClient - knows which resources it should request and how
- CnbApiParser - parsing logic

## ToDos

aka wasn't implemented in the dedicated timeframe but should be included in real production system

- logging using ILogger
- fallback for the case of unavailability of the cnb website - should trigger production alarm
- better handling of working days - e. g. at the moment, Easter is not included in public holidays as it is a moving one
- cache locking to avoid multiple requests passing the update test, that would result in multiple requests to CNB
- extract parameters to the config file - CNB urls, wait time on unsuccessfull cache update, ...
- simple integration testing
- CI pipeline that builds the project and runs unit tests (for example using GitHub actions)

## Open questions

- how to handle change of winter/summer time in CZE - does CNB always publish the data at 2:30 PM? If so, this logic should be included into program.
- the note in monthly updated currencies (https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/) states that "exchange rate declared on Friday 26 February is valid for each day between 1 March and 31 March". So on Saturday 27th February, should we display the most current rates we have, or still the older ones, because the new ones only apply in March according to the description.
- does the listing always appear exactly at 2:30pm? What is the best way to handle if it is unrealiable? - currently we try the request again after 5 minutes.

## Usage in real life scenario

This example was implemented into console application, where in memory caching doesn't make sense since application is recreated on each use.

The ExchangeRateProvider can be easily plugged into larger project where its functionality is needed.

In the ideal case, the ExchangeRateProvider would be used a service, where a scheduler or service thread takes responsibility of requests to CNB api and updating our data in memory/external cache/(no)sql db. The users of the service then could easily call it and quickly get the (filtered) data.
