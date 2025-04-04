# ExchangeRateUpdater

- A simple Console App (written in .NET 8)
- Exchange rates data are retrieved from [ČNB's official website](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt)
  - According to official ČNB docs, rates are updated daily after 2:30 PM CET
- Rates are cached in memory for 1 hour
  - Since this is a Console App, caching doesn't provide significant benefits right now, but as this evolves into a real application, caching could be easily leveraged
- A basic retry mechanism has been introduced for HttpClient to handle transient errors
- Simple Dependency Injection (DI) has been introduced for easier testing and mocking
- Most scenarios are covered by unit tests
