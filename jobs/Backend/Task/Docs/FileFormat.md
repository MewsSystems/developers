### Format of the foreign exchange market rates
URL for access:
https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=DD.MM.YYYY

or

https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt (without parameter, only for current exchange rates)

DD - day
MM - month
YYYY - year

Description of TXT format:

The first line of the file consists of the date for which the exchange rate was declared in DD.MM.YYYY format. After the date there are 2 spaces and a # sign together with the sequence number of the rates published within the year. Example 1st line:

03.Jan.2000 #1

This line is followed by the exchange rates, with the first line consisting of a header in the following form:
Country|Currency|Amount|Code|Rate

The other lines contain the data themselves. Each line contains one currency. The individual figures are separated with a '|' sign; in the case of figures with a decimal part the decimal place separator is a '.' sign. Example for a specific currency:

Australia|dollar|1|AUD|23.282

The foreign exchange market rate data are followed by a blank line separating the data from the calculated rates (only for years 1999-2001). These start with the header:

Country|Currency|Amount|Code|Rate

The data themselves follow on the subsequent lines (the separator being a '|' sign).

Example line of data:

Belgium|frank|100|BEF|89.762