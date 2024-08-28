@ExchangeRates @CzechNationalBankApi @Cache
Feature: Get

@HappyPath
Scenario: Get all exchange rates retrieves a list of all exchange rates supported by the application
	Given Czech National Bank Api is able to retrieve the exchange rates
	When all exchange rates are requested
	Then HttpStatus 200 is returned
	And a call to Czech National Bank Api to get the exchange rates is done
	And all Czech National Bank exchange rates are stored in cache
	And response is
	"""
    {
      "exchangeRates": [
        {
          "sourceCurrency": {
            "code": "EUR"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 25.005
        },
        {
          "sourceCurrency": {
            "code": "JPY"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 0.15021
        },
        {
          "sourceCurrency": {
            "code": "THB"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 0.63008
        },
        {
          "sourceCurrency": {
            "code": "TRY"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 0.71979
        },
        {
          "sourceCurrency": {
            "code": "USD"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 23.223
        }
      ]
    }
	"""
    
@HappyPath
Scenario: Exchange rates are retrieved from cache the second time are requested
	Given Czech National Bank Api is able to retrieve the exchange rates
	When all exchange rates are requested
    When all exchange rates are requested
	Then HttpStatus 200 is returned
	And only one call to Czech National Bank Api to get the exchange rates is done
	And all Czech National Bank exchange rates are stored in cache
	And response is
	"""
    {
      "exchangeRates": [
        {
          "sourceCurrency": {
            "code": "EUR"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 25.005
        },
        {
          "sourceCurrency": {
            "code": "JPY"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 0.15021
        },
        {
          "sourceCurrency": {
            "code": "THB"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 0.63008
        },
        {
          "sourceCurrency": {
            "code": "TRY"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 0.71979
        },
        {
          "sourceCurrency": {
            "code": "USD"
          },
          "targetCurrency": {
            "code": "CZK"
          },
          "value": 23.223
        }
      ]
    }
	"""
    
@UnhappyPath
Scenario: Internal Server Error is returned when Czech National Bank Api is down
	Given Czech National Bank Api fails to retrieve the exchange rates
	When all exchange rates are requested
	Then HttpStatus 500 is returned
	Then a call to Czech National Bank Api to get the exchange rates is done
	And no Czech National Bank exchange rates are added to the cache
    
@UnhappyPath
Scenario: Request Timeout is returned when Czech National Bank Api times out
	Given Czech National Bank Api times out when retrieving the exchange rates
	When all exchange rates are requested
	Then HttpStatus 408 is returned
	Then a call to Czech National Bank Api to get the exchange rates is done
	And no Czech National Bank exchange rates are added to the cache
