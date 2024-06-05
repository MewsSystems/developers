### Big Picture

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
		


One approach here will be to extract data for the currencies specified from the text file:
- parse the text file  