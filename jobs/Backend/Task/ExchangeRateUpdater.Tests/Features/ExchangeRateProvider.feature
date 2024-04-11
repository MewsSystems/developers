Feature: ExchangeRateProvider
Feature: ExchangeRateProvider

Retrieve a list of rates from Czech National Bank, given a list of currency codes

Link to a feature: [ExchangeRateProvider](ExchangeRateUpdater.Tests/Features/ExchangeRateProvider.feature)


Scenario: When retrieving rates for a given list of currencies, it should return matching api currency rates
    Given ApiClient returns rates for the currencies 'EUR,JPY,THB,TRY,USD,BRL,AOA]'
    And Default culture is EN
    And Cache is empty
    When Getting the rates for the currencies 'USD,EUR,CZK,JPY,KES,RUB,THB,TRY,XYZ'
    Then The rates for the following currencies should be returned 'EUR,JPY,THB,TRY,USD'
    And Cache gets populated

Scenario: When Cache contains a value, it should return it
    Given Cache returns rates for the currencies 'EUR,JPY,THB,TRY,USD,BRL,AOA]'
    When Getting the rates for the currencies 'USD,EUR,CZK,JPY,KES,RUB,THB,TRY,XYZ'
    Then The rates for the following currencies should be returned 'EUR,JPY,THB,TRY,USD'
    And API should not be consumed

Scenario: When Long term Cache contains a value, and API throws an exception, it should return values from long term cache
    Given Short term cache is empty
    And Long term cache returns rates for the currencies 'EUR,JPY,THB,TRY,USD,BRL,AOA]'
    And Default culture is EN
    And API has issues
    When Getting the rates for the currencies 'USD,EUR,CZK,JPY,KES,RUB,THB,TRY,XYZ'
    Then The rates for the following currencies should be returned 'EUR,JPY,THB,TRY,USD'
    And Long term cache should be called