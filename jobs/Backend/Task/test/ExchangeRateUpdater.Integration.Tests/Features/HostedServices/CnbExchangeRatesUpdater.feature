@CnbExchangeRatesUpdater @FeatureFlags @CzechNationalBankApi @Cache @CzechNationalBankApi
Feature: CnbExchangeRatesUpdater

Background: 
	Given the hosted service is executed during 1 second
	
@HappyPath
Scenario: All exchange rates are stored in the cache after hosted service execution
	Given CnbExchangeRatesUpdater is enabled
	And Czech National Bank Api is able to retrieve the exchange rates
	When CnbExchangeRatesUpdater is executed
	Then a call to Czech National Bank Api to get the exchange rates is done
	And all Czech National Bank exchange rates are stored in cache

@UnhappyPath
Scenario: Processing is skipped when hosted service is disabled
	Given CnbExchangeRatesUpdater is disabled
	When CnbExchangeRatesUpdater is executed
	Then no call is performed to Czech National Bank Api to get the exchange rates
	And no Czech National Bank exchange rates are added to the cache
	
@UnhappyPath
Scenario: Hosted service does not crash if Czech National Bank Api is down
	Given CnbExchangeRatesUpdater is enabled
	And Czech National Bank Api fails to retrieve the exchange rates
	When CnbExchangeRatesUpdater is executed no exception is thrown
	Then a call to Czech National Bank Api to get the exchange rates is done
	And no Czech National Bank exchange rates are added to the cache

