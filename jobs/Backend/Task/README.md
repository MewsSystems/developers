# Proposed API solution
This API is created with the goal of being extensible. It allows to get the current exchange rates from the CNB, and makes them available through the API.
This could be extended adding new providers with new parsers.
The appsettings.json should include the allowed sources set by base currency and allowed currencies.

It will have only one GET endpoint "ExchangeRate" that will take a mandatory parameter "currencies" and an optional parameter "baseCurrency" which is currently defaulting to "CZK" as it's the only one implemented at the moment