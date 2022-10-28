## A set of unrelated data that may be needed in the implementation of a task.

- [ ] [Format of the txt file](FileFormat.md)
#### CNB exchange rates endpoints: 
  - [By the year](https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/year.txt)
  - [Today](https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt)
  - [By the Date](https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt?date=30.08.2020)
  - [By the Date (form the previous provider's implementation)](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt) (e.g. EUR, USD). Exchange rates of commonly traded currencies are declared every working day after 2.30 p.m. and are valid for the current working day and, where relevant, the following Saturday, Sunday or public holiday (for example, an exchange rate declared on Tuesday 23 December is valid for Tuesday 23 December, the public holidays 24–26 December, and Saturday 27 December and Sunday 28 December).
    - [Example](fx_rates.txt)
  - [Other currencies rates](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year=2022&month=9) (e.g. KES, RUB). The exchange rates of other currencies are declared on the last working day of the month and are valid for the entire following month (for example, an exchange rate declared on Friday 26 February is valid for each day between 1 March and 31 March).
    - [Example](fx_rates-others.txt)

#### Notes:
- [ ] The data is in the form of a text file, which is parsed and converted to a C# model.
- [ ] The data is updated daily or monthly, depends on the source.