### Big Picture

The way that I understand the problem : 
- we will get the exchange rate between Czech Krona and other currencies from the National Bank of Czech Republic.
	- on the bank website we will find the foreign currency and the equivalent amount of Czech Krona needed to aquire 1 (or more) unit of the foreign currency
		- so the following line : "USA|dollar|1|USD|22.366" - tells us that 1 USD is equivalent to 22.366 CZK
		- which means that >>>  USD/CZK indicates the amount of CZK you get for 1 USD.
		- eqivalent to : SourceCurrency is USD and TargetCurrency is CZK !

In our current problem we can assume that the "TargetCurrency" is "always" CZK and the "SourceCurrency" is the currency that we are interested in (will be passed in as a parameter).


## Approach

- find the location where we can extract the exchage data from the web (bank web address)
	- found it here : [Central bank exchange rate fixing](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/index.html?date=04.06.2024)
	- note that the exchage rate is date dependent
	- the date is in the 'normal' :-) European format (day.month.year) --> dd.mm.yyyy
		- one way will be to parse the table that is present on the web page
		- or
		- there is a link at the bottom of the table that responds to an url and returns data in some sort of text format
		- the link is : https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=08.01.2024
		- the data is in the following format:
			- 1st line is the date
			- 2nd line is the header
			- the rest of the lines are the data - pipe separated (in the same way as the header)

- once we get the data from the Central Bank:
	- we will split it (by hte \n character) into lines
	- will validate each line 
		- making sure that has the right amount of fields
		- the values in the fields are of the right type
	- after that we will create an object of type ExchangeRate for each line where:
		- the source-currency is the foreight currency fom the line
		- the target-currency is CZK
		- and the rate is the exchange rate from the line (we might need to divide it for currencies that ore converted to CZK in larger amounts : 10, 100, 1000 ...)
		- this is the final set of data that we will pass in the constructor of the ExchangerateProvider class.
		- the method 'GetExchangeRates' will extract only the rates for the currencies that are passed in as a parameter (the ones defined in the Program.cs file).