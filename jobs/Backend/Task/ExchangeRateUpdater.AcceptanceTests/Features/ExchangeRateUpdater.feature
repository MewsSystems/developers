Feature: ExchangeRateUpdater

ExchangeRateUpdater is an API that allows retrieving exchange rates from the Czech National Bank

Scenario: ExchangeRateUpdater returns today's exchange rates
	Given we have the ExchangeRateUpdater Api running
	When we call the api/exchange-rates endpoint
	Then the result should be today's exchange rates