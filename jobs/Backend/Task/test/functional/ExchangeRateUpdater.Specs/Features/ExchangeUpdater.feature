Feature: ExchangeUpdater

Scenario: ExchangeUpdater prints out the exhange rates when they are provided by the source
	Given exchange rate for USD / EUR is 1.0
	When I run the ExchangeUpdater
	Then the program prints out "USD/EUR=1.0"
	And the program doesn't print out "EUR/USD=1.0"
