# Exchange Rate Provider

The solution implements an Exchange Rate Provider for the Czech National Bank. The code is structured in a way that the bank API client can easily be replaced with a different one.

These are the main components of this solution:

## `ExchangeRateProvider` class

This class uses an instance of IBankApiClient to retrieve the exchange rates in relation to a base currency. It then builds a list of exchange rates between source and target currencies, and filters the list according to the currencies specified as a parameter. It assumes the default target currency is `CZK`.

## `CnbBankApiClient` class:
Typed HTTP client that implements `IBankApiClient` and connects to the Czech National Bank API to retrieve the dates for a specific date. Uses caching and a retry policy.

## `TestApp` application:
Console app to test the exchange rate provider.

## To run the Test console application:

```console
cd .\src\TestApp
dotnet run
```

The output will be something similar to this:

```console
EUR/CZK=24.575
JPY/CZK=0.14496
THB/CZK=0.61828
TRY/CZK=0.69917
USD/CZK=22.559
```