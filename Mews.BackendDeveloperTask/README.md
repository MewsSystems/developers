# Mews Backend Developer Task - Exchange Rates

## Specification

The task is to implement an ExchangeRateProvider for Czech National Bank. Find data source on their web - part of the task is to find the source of the exchange rate data and a way how to extract it from there.

It is up to you to decide which technology (from .NET family) or package to use. Any code design changes/decisions to the provided skeleton are also completely up to you.

The solution has to be buildable, runnable and the test program should output the obtained exchange rates.

Goal is to implement a fully functional provider based on real world public data source of the assigned bank.

To submit your solution, just open a new pull request to this repository.

Please write the code like you would if you needed this to run on production environment and had to take care of it long-term.

## Tasks

- [x] Find data source on their web - part of the task is to find the source of the exchange rate data
- [x] Choose technology for implementation
- [x] Add unit tests for the exchange rate provider
- [x] Implement unit tests with mock data provider
- [ ] Add unit tests for the real world data provider retriever/parser
- [ ] Implement real world data provider and example CLI
- [ ] Perform acceptance tests per below listed acceptance criteria
- [ ] Add build, test and usage instructions to readme

## Acceptance Criteria

### From readme

- [ ] Solution is buildable and runnable
- [ ] Test program outputs the obtained exchange rates
- [ ] Provider is fully functional and sources real world public data from CNB
- [ ] Code is of a long-term maintainable production standard

### From example file

- [ ] Should return exchange rates among the specified currencies that are defined by the source. But only those defined by the source
- [ ] Do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK" do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". 
- [ ] If the source does not provide some of the currencies, ignore them.

## Data Source

<img alt="CNB logo" src="https://www.cnb.cz/export/system/modules/cz.nelasoft.opencms.cnb/resources/img/LOGO-2RA_RGB.svg" width="200px" />

[overview](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/)

[daily text file](https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt)

## Future enhancements

- [Currency](./Mews.BackendDeveloperTask.ExchangeRates/Currency.cs) may benefit from being a ["smart" enum](https://codeblog.jonskeet.uk/2006/01/05/classenum/)
- When CNB or other exchange rate providers provide an API-side filter, we should create new implementations of [IExchangeRateProvider](./Mews.BackendDeveloperTask.ExchangeRates/IExchangeRateProvider.cs) that pass the currencies as part of the request to reduce payload size and execution time to only that which is needed.