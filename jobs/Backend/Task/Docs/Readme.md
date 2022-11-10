## This test assignment is about the Czech National Bank (CNB) exchange rates.

### General information about the CNB data source:
The CNB bank publishes the exchange rates to the czech crown(CZK) in a CSV file.
The CSV files are published at the following URLs:
- Main currencies: https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=DD.MM.YYYY
  The exchange rates of main currencies are published every working day after 2.30 p.m. They are valid for the current working day and, when it is relevant, for the following Saturday, Sunday or public holiday (for example, an exchange rate declared on Tuesday December 23 is valid for Tuesday December 23, the public holidays December 24–26, Saturday December 27 and Sunday December 28).
  - [Example.txt](fx_rates.txt)
- Other currencies: https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year=YYYY&month=MM (e.g. KES, RUB).
  The exchange rates of other currencies are published on the last working day of month and are valid for the entire following month (for example, an exchange rate declared on Friday February 26 is valid for each day between March 1 and March 31).
  - [Example.txt](fx_rates-others.txt)

The CSV file contains the following columns: * Country - the name of a country * Currency - the name of a currency * Amount - an amount of currency in the unit of the currency * Code - ISO 4217 3-letter code of a currency * Rate - the exchange rate to the Czech Koruna.

---

This project is a .NET Core console application. The application should be able to download the CSV file from the CNB website, parse the data and print it into the console in the correct format.

#### In addition to the general goals outlined in the Readme file, I have set the following goals, because I considered them to be of crucial importance for the project:
1. The application should effectively search for rates in the files that the bank provides at the two endpoints I found. (in this way we will find the maximum number of rates, not only the most popular ones).
2. The results should be cached.
3. Build the app in a way that allows one to test classes in isolation
4. Design the application so that if new functions are added in the future, they have no or minimal impact on the existing code.

Initially I planned just to find an existing PR that would satisfy all or at least most of these conditions, because the best solution is when you don't need to code anything at all, but unfortunately studying a few recent PRs did not yield the desired result. But I would still like to thank John Wylie, Mickaël Derriey, husain-al-radhi and Matic Novak for some ideas from their PRs.

---

### Things I would like to implement, but I haven't:
- [ ] Add logging, integration tests, more test scenarios in unit tests;
- [ ] Reset cache at 2:30 p.m., because that is when files are updated, according to the CNB bank, or even add Http Watcher to update cache when file is updated;
- [ ] Add a feature to search for rates by date;
- [ ] Add a CI/CD pipeline;
- [ ] Add a docker-compose file.