# Mews backend developer task

The task was to implement an [ExchangeRateProvider](Task/ExchangeRateProvider.cs) for Czech National Bank.

As there were no specific requirements on how it should be implement or if my OOP skills are meant to be tested, I went with the most simple, straightforward, not overengineered solution

If my OOP skills would have been tested, I would create separate provider for CNB data, create interface and etc
Logging is another thing which is needed for the full solution, I added some comments in places I would add logging

The CNB data source I used is "https://www.cnb.cz/en/financial_markets/foreign_exchange_market/exchange_rate_fixing/daily.txt"
It was the most easy data source to parse, and I couldn't find any additional data sources which provide more currency pairs, as EUR/USD, the only ones it has is CZK as the only quote currency

