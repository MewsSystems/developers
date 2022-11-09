## 
This test assignment is about the Czech National Bank (CNB) exchange rates. 
The CNB publishes the exchange rates to the Czech Koruna (CZK) in a CSV file. 
The CSV file is published at the following URLs: 
 - Main currencies: https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt?date=DD.MM.YYYY 
 The exchange rates of main currencies are declared every working day after 2.30 p.m. They are valid for the current working day and, where relevant, the following Saturday, Sunday or public holiday (for example, an exchange rate declared on Tuesday 23 December is valid for Tuesday 23 December, the public holidays 24–26 December, and Saturday 27 December and Sunday 28 December).
   - [Example.txt](fx_rates.txt)
 - Other currencies: https://www.cnb.cz/en/financial-markets/foreign-exchange-market/fx-rates-of-other-currencies/fx-rates-of-other-currencies/fx_rates.txt?year=YYYY&month=MM (e.g. KES, RUB). 
 The exchange rates of other currencies are declared on the last working day of the month and are valid for the entire following month (for example, an exchange rate declared on Friday 26 February is valid for each day between 1 March and 31 March).
   - [Example.txt](fx_rates-others.txt)

> The CSV file contains the following columns: * Country - the name of the country * Currency - the name of the currency * Amount - the amount of the currency in the unit of the currency * Code - the ISO 4217 3-letter code of the currency * Rate - the exchange rate to the Czech Koruna.

This project is a .NET Core console application. The application should be able to download the CSV file from the CNB website, parse the data and print it into the console in the correct format.

#### In addition to the general goals outlined in the Readme file, I have set the following goals, because I considered them to be of crucial importance for the project: 
1. The application should effectively search for rates in the files that the bank provides at the two endpoints I found. (in this way we will find the maximum number of rates, not only the most popular ones)
2. The results should be cached.
3. Build the app in a way to be able to test classes in isolation.
4. Design the application so that if new functions are added in the future, they have no or minimal impact on the existing code. 

At first, I planned to just find an existing PR that would satisfy all or at least most of these conditions, because the best solution is when you don't need to code anything at all, but unfortunately studying a few recent PRs did not yield the desired result. But I would still like to thank John Wylie, Mickaël Derriey, husain-al-radhi and Matic Novak for some ideas from their PRs. 

### Things I would like to implement, but I haven't:
- [ ] Add logging, integration tests, more test scenarios in unit tests
- [ ] Reset cache at 2:30 p.m., because that is when files are updated, according to CNB, or even add Http Watcher to update cache when file is updated.
- [ ] Add a feature to search for rates by date
- [ ] Add a CI/CD pipeline
- [ ] Add a docker-compose file